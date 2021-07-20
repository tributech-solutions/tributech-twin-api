using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.VocabularyAPI {
    public class CatalogApiAuthHandler : DelegatingHandler {
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<CatalogApiAuthHandler> _logger;

		public CatalogApiAuthHandler(IHttpContextAccessor httpContextAccessor, ILogger<CatalogApiAuthHandler> logger) {
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			// Extract the access token from the current request and pass it to the Catalog-API
			string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);
			if (string.IsNullOrEmpty(accessToken)) {
				_logger.LogWarning("Request failed: Access token to be passed through to Catalog-API is missing.");
				return new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}
	}
}
