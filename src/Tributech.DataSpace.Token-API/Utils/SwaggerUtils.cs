using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Tributech.DataSpace.Common.Auth.ApiAuth;

namespace Tributech.DataSpace.TwinAPI.Utils {
	public static class SwaggerUtils {

		private const string SWAGGER_ENDPOINT_DESCRIPTION = "Twin API";
		private const string SWAGGER_ENDPOINT_OPENAPI_JSON_PATH = "/swagger/v1/swagger.json";
		private const string SWAGGER_API_VERSION = "v1";
		private const string SWAGGER_API_TITLE = "Tributech DataSpace Twin API";
		private const string SWAGGER_API_DESCRIPTION = "API for managing twin instances.";

		public static void AddSwaggerCustom(this IServiceCollection services, ApiAuthOptions apiAuthOptions) {

			services.AddSwaggerGen(c => {

				c.SwaggerDoc(SWAGGER_API_VERSION, new OpenApiInfo {
					Title = SWAGGER_API_TITLE,
					Version = SWAGGER_API_VERSION,
					Description = SWAGGER_API_DESCRIPTION
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);

				c.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme {
					Type = SecuritySchemeType.OAuth2,
					In = ParameterLocation.Header,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					Name = JwtBearerDefaults.AuthenticationScheme,
					Flows = new OpenApiOAuthFlows {
						// only authorization code since we need context of user
						AuthorizationCode = new OpenApiOAuthFlow() {
							TokenUrl = new Uri(apiAuthOptions.TokenUrl),
							AuthorizationUrl = new Uri(apiAuthOptions.AuthorizationUrl),
							Scopes = apiAuthOptions.Scopes.ToDictionary(s => s, s => s)
						}
					}
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement() {{
					new OpenApiSecurityScheme{
						Reference = new OpenApiReference{
							Type = ReferenceType.SecurityScheme,
							Id = nameof(SecuritySchemeType.OAuth2)
						},
					},
					apiAuthOptions.Scopes
				}});
			});
			services.AddSwaggerGenNewtonsoftSupport();

		}

		public static void UseSwaggerCustom(this IApplicationBuilder app, ApiAuthOptions apiAuthOptions) {
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint(SWAGGER_ENDPOINT_OPENAPI_JSON_PATH, SWAGGER_ENDPOINT_DESCRIPTION);
				c.RoutePrefix = string.Empty;
				c.UseApiAuth(apiAuthOptions);
			});

		}

	}
}
