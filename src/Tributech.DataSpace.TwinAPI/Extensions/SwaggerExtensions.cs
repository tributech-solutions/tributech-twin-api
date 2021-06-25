using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Tributech.DataSpace.TwinAPI.Options;
using System;
using System.Linq;

namespace Tributech.DataSpace.TwinAPI.Extensions {
	public static class SwaggerUIOptionsExtensions {
		public const string JwtBearerAuthenticationScheme = "Bearer";

		/// <summary>
		/// Add OAuth2 client credentials and authorization code flow authorization to swagger-ui
		/// based on configuration (e.g. client, scopes,...) from <see cref="ApiAuthOptions"/>.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="apiAuthOptions"></param>
		public static void AddApiAuth(this SwaggerGenOptions options, ApiAuthOptions apiAuthOptions) {
			options.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme {
				Type = SecuritySchemeType.OAuth2,
				In = ParameterLocation.Header,
				Scheme = JwtBearerAuthenticationScheme,
				Name = JwtBearerAuthenticationScheme,
				Flows = new OpenApiOAuthFlows {
					ClientCredentials = new OpenApiOAuthFlow() {
						TokenUrl = new Uri(apiAuthOptions.TokenUrl),
						Scopes = apiAuthOptions.Scopes.ToDictionary(s => s, s => s)
					},
					AuthorizationCode = new OpenApiOAuthFlow() {
						TokenUrl = new Uri(apiAuthOptions.TokenUrl),
						AuthorizationUrl = new Uri(apiAuthOptions.AuthorizationUrl),
						Scopes = apiAuthOptions.Scopes.ToDictionary(s => s, s => s)
					}
				}
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement() {{
					new OpenApiSecurityScheme{
						Reference = new OpenApiReference{
							Type = ReferenceType.SecurityScheme,
							Id = nameof(SecuritySchemeType.OAuth2)
						},
					},
					apiAuthOptions.Scopes
				}});
		}

		/// <summary>
		/// Use OAuth2 at swagger-ui based on configuration (e.g. client, scopes,...) from <see cref="ApiAuthOptions"/>.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="apiAuthOptions"></param>
		public static void UseApiAuth(this SwaggerUIOptions options, ApiAuthOptions apiAuthOptions) {
			options.OAuthClientId(apiAuthOptions.ClientId);
			options.OAuthScopes(apiAuthOptions.Scopes);
			options.OAuthUsePkce();
		}
	}
}
