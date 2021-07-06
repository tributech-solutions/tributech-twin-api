﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Infrastructure;
using Tributech.DataSpace.TwinAPI.Options;
using Tributech.DataSpace.TwinAPI.Utils;
using FluentValidation.AspNetCore;
using Tributech.DataSpace.TwinAPI.Validators;

namespace Tributech.DataSpace.TwinAPI {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddOptions(Configuration, out ApiAuthOptions apiAuthOptions);

			//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			//	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
			//		options.Authority = apiAuthOptions.Authority;
			//		options.Audience = apiAuthOptions.Audience;
			//	});

			services.AddHealthChecks();
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddInfrastructure(Configuration);
			services.AddValidators(Configuration);

			// We use Newtonsoft as our Neo4j client library requires it and we dont want to mix two frameworks.
			services.AddControllers().AddNewtonsoftJson();
			services.AddSwaggerCustom(apiAuthOptions);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ApiAuthOptions> apiAuthOptionsAccessor) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			//app.UseAuthentication();
			//app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/health");
			});

			app.UseSwaggerCustom(apiAuthOptionsAccessor.Value);
		}
	}
}
