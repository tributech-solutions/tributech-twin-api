using System;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tributech.DataSpace.TwinAPI.Options;

namespace Tributech.DataSpace.TwinAPI.Extensions {
	public static class HttpClientFactoryServiceCollectionExtensions {

		/// <summary>
		/// Helper to create a typed http client with automated token based authentication.
		/// We use the client from the configuration to retrieve access tokens with OAuth2 client credentials flow.
		/// The access tokens are cached an set as bearer token at every request made by the typed http client.
		/// </summary>
		/// <typeparam name="TClient"> The type of the typed client. 
		/// They type specified will be registered in the servicecollection as a transient service. 
		/// See Microsoft.Extensions.Http.ITypedHttpClientFactory`1 for more details about authoring typed clients.</typeparam>
		/// <typeparam name="TImplementation">The implementation type of the typed client. 
		/// They type specified will be instantiated by the Microsoft.Extensions.Http.ITypedHttpClientFactory`1</typeparam>
		/// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection.</param>
		/// <param name="configure">A delegate that is used to configure the <see cref="ClientAuthOptions"/>.</param>
		/// <param name="clientAuthOptionsName">The name for the named options instance create for <see cref="ClientAuthOptions"/>.</param>
		/// <returns>An Microsoft.Extensions.DependencyInjection.IHttpClientBuilder that can be used to configure the client.</returns>
		public static IHttpClientBuilder AddHttpClientWithClientCredentialsAuth<TClient, TImplementation>(this IServiceCollection services, Action<ClientAuthOptions> configure, string? clientAuthOptionsName = null)
			where TClient : class
			where TImplementation : class, TClient {
			string optionsName = string.IsNullOrEmpty(clientAuthOptionsName) ? typeof(TImplementation).Name : clientAuthOptionsName;

			if (configure != null) {
				// configure and create named options instance
				services.Configure(optionsName, configure);
			}

			// configure access token management using our named options instance of the client auth options
			services.AddSingleton<IConfigureOptions<AccessTokenManagementOptions>, ConfigureAccessTokenManagementOptions>(sp => {
				ClientAuthOptions options = sp.GetRequiredService<IOptionsMonitor<ClientAuthOptions>>().Get(optionsName);
				return new ConfigureAccessTokenManagementOptions(options);
			});
			services.AddAccessTokenManagement();

			// create typed http client with access token handler
			IHttpClientBuilder client = services.AddHttpClient<TClient, TImplementation>(
				(sp, httpClient) => {
					ClientAuthOptions options = sp.GetRequiredService<IOptionsMonitor<ClientAuthOptions>>().Get(optionsName);
					httpClient.BaseAddress = new Uri(options.ApiEndpoint);
				})
				.AddClientAccessTokenHandler(optionsName);

			return client;
		}

		/// <summary>
		/// Helper to add <see cref="ClientAccessTokenHandler"/> configured with our named options instance of the client auth options.
		/// The client id from the options is used to retrieve the token handler setup in access token management.
		/// </summary>
		/// <param name="httpClientBuilder"></param>
		/// <param name="optionsName"></param>
		/// <returns></returns>
		public static IHttpClientBuilder AddClientAccessTokenHandler(this IHttpClientBuilder httpClientBuilder, string optionsName) {
			return httpClientBuilder.AddHttpMessageHandler(sp => {
				IAccessTokenManagementService accessTokenManagementService = sp.GetRequiredService<IAccessTokenManagementService>();
				ClientAuthOptions options = sp.GetRequiredService<IOptionsMonitor<ClientAuthOptions>>().Get(optionsName);
				return new ClientAccessTokenHandler(accessTokenManagementService, options.ClientId);
			});
		}
	}

	/// <summary>
	/// Helper class to apply instance of named options <see cref="ClientAuthOptions"/> to the <see cref="AccessTokenManagementOptions"/>.
	/// This helps use to provide simply DI based setup.
	/// See also https://andrewlock.net/avoiding-startup-service-injection-in-asp-net-core-3/#using-iconfigureoptions-to-configure-options-for-identityserver
	/// </summary>
	public class ConfigureAccessTokenManagementOptions : IConfigureOptions<AccessTokenManagementOptions> {
		private readonly ClientAuthOptions _authOptions;

		public ConfigureAccessTokenManagementOptions(IOptions<ClientAuthOptions> authOptions) : this(authOptions.Value) {
		}

		public ConfigureAccessTokenManagementOptions(ClientAuthOptions authOptions) {
			_authOptions = authOptions;
		}

		public void Configure(AccessTokenManagementOptions options) {
			// we use the client id from the options as key
			options.Client.Clients.Add(_authOptions.ClientId, new ClientCredentialsTokenRequest {
				Address = _authOptions.TokenUrl,
				ClientId = _authOptions.ClientId,
				ClientSecret = _authOptions.ClientSecret,
				Scope = string.Join(' ', _authOptions.Scopes)
			});
		}
	}
}
