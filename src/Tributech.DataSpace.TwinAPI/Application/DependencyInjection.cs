using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Tributech.DataSpace.TwinAPI.Application {
	public static class DependencyInjection {
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) {
			return services;
				//.AddSingleton<IAgentLinker, AgentLinker>()
		}
	}
}
