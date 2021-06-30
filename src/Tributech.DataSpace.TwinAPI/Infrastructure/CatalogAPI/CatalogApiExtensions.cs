using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Tributech.DataSpace.TwinAPI.Extensions;
using Tributech.DataSpace.TwinAPI.Options;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.CatalogAPI {
	public static class CatalogApiExtensions {

		public static IServiceCollection AddCatalogAPIClient(this IServiceCollection services, Action<ClientAuthOptions> configure) {
			var jitterer = new Random();
			services.AddHttpClientWithClientCredentialsAuth<CatalogAPIClient, CatalogAPIClient>(configure)
				// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2#use-polly-based-handlers
				// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
				// retry 3 times, exponential back-off plus some jitter
				.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))))
				// after 5 failed attempts fail immediately (without placing actual request), wait 30sec till trying next actual request
				.AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

			return services;
		}
	}
}
