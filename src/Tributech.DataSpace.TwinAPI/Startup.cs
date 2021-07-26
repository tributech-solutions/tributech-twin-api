using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
						// globally register 500 internal server error response type
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
			//// Only include exception details in a development environment. There's really no nee
			//// to set this as it's the default behavior. It's just included here for completeness :)
			//options.IncludeExceptionDetails = (ctx, ex) => _environment.IsDevelopment();

			//// This will map NotImplementedException to the 501 Not Implemented status code.
			//options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

			//// Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
			//options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
		}
	}
}
