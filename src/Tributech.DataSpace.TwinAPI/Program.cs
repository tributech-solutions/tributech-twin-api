using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;

namespace Tributech.DataSpace.TwinAPI {
	public class Program {
		public static async Task Main(string[] args) {
			IHost host = CreateHostBuilder(args).Build();
			await host.BootsrapNeo4j();
			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				});
	}
}
