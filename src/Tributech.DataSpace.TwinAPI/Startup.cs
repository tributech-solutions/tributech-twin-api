using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Tributech.DataSpace.TwinAPI.Application;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Infrastructure;
using Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j;
using Tributech.DataSpace.TwinAPI.Options;
using Tributech.DataSpace.TwinAPI.Utils;
using Tributech.Dsk.CatalogApi.Client;

namespace Tributech.DataSpace.TwinAPI {
	public class Startup {
		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
			Configuration = configuration;
			_environment = environment;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddOptions(Configuration, out ApiAuthOptions apiAuthOptions);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
					options.Authority = apiAuthOptions.Authority;
					options.Audience = apiAuthOptions.Audience;
				});

			services.AddHealthChecks()
					.AddCheck<Neo4jHealthCheck>(nameof(Neo4jHealthCheck), tags: new[] { "neo4j", "db" });
			services.AddRouting(options => options.LowercaseUrls = true);

			services.AddApplication();
			services.AddInfrastructure(Configuration);
			
			services.AddProblemDetails(ConfigureProblemDetails)
					.AddControllers(cfg => {
						// globally register response types
						cfg.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status401Unauthorized));
						cfg.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
					})
					.AddNewtonsoftJson() // We use Newtonsoft as our Neo4j client library requires it and we dont want to mix two frameworks.
					.AddProblemDetailsConventions();
			services.AddSwaggerCustom(apiAuthOptions);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ApiAuthOptions> apiAuthOptionsAccessor) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			app.UseProblemDetails();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/health");
			});

			app.UseSwaggerCustom(apiAuthOptionsAccessor.Value);
		}

		private void ConfigureProblemDetails(ProblemDetailsOptions options) {
			//include/exclude exceptions, adapt mappings,...
			options.Map(
				(HttpContext _, ApiException ex) => ex.StatusCode == StatusCodes.Status401Unauthorized,
				(HttpContext _, ApiException ex) => {
					const int statusCode = StatusCodes.Status401Unauthorized;
					return new ProblemDetails {
						Status = statusCode,
						Title = ReasonPhrases.GetReasonPhrase(statusCode),
						Type = $"https://httpstatuses.com/{statusCode}"
					};
				});
		}
	}
}
