using Tributech.DataSpace.TwinAPI.Options;

namespace Tributech.DataSpace.TwinAPI.Extensions {
	public static class ServiceCollectionExtensions {
		/// <summary>
		/// Helper to configure options with some handy defaults and out param for instance (e.g. for <see cref="ApiAuthOptions"/>).
		/// </summary>
		/// <typeparam name="TOptions">The options type.</typeparam>
		/// <param name="services">The service collection to register options at.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="options">Instance of the options.</param>
		/// <param name="configSectionName">(Optional) The config section name. Default: Name of TOptions type.</param>
		/// <param name="optionsInstanceName">(Optional) The name of the options instance. Default: "" (as defined in <see cref="Microsoft.Extensions.Options.Options.DefaultName"/>)</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, out TOptions options, string? configSectionName = null, string optionsInstanceName = "")
			where TOptions : class, new() {
			configSectionName ??= typeof(TOptions).Name;
			IConfigurationSection apiAuthOptionsSection = configuration.GetSection(configSectionName);
			options = apiAuthOptionsSection.Get<TOptions>() ?? new TOptions();
			services.AddOptions<TOptions>(optionsInstanceName).Bind(apiAuthOptionsSection);
			return services;
		}

		/// <summary>
		/// Helper to configure options with some handy defaults (e.g. for <see cref="ApiAuthOptions"/>).
		/// </summary>
		/// <typeparam name="TOptions">The options type.</typeparam>
		/// <param name="services">The service collection to register options at.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="configSectionName">(Optional) The config section name. Default: Name of TOptions type.</param>
		/// <param name="optionsInstanceName">(Optional) The name of the options instance. Default: "" (as defined in <see cref="Microsoft.Extensions.Options.Options.DefaultName"/>)</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, string? configSectionName = null, string optionsInstanceName = "")
			where TOptions : class, new()
			=> AddOptions<TOptions>(services, configuration, out _, configSectionName, optionsInstanceName);
	}
}
