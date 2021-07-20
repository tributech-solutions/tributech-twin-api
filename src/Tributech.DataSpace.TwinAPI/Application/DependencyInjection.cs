using Microsoft.Extensions.DependencyInjection;
using Tributech.DataSpace.TwinAPI.Application.Schema;

namespace Tributech.DataSpace.TwinAPI.Application {
	public static class DependencyInjection {
		public static IServiceCollection AddApplication(this IServiceCollection services) {
			return services
				.AddSingleton<ISchemaService, SchemaService>()
				.AddScoped<ITwinService, TwinService>();
		}
	}
}
