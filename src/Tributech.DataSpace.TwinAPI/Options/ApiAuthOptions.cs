using System;

namespace Tributech.DataSpace.TwinAPI.Options {
	/// <summary>
	/// Authentication/authorization options for securing APIs (and to pre-configure Swagger-UI for gaining access to API).
	/// </summary>
	public class ApiAuthOptions {
		private string? _tokenUrl;
		private string? _authorizationUrl;

		/// <summary>
		/// The authority (keycloak realm) we trust.
		/// </summary>
		public string Authority { get; set; } = "";

		/// <summary>
		/// The OpenID Connect token url (e.g. used to retrieve access tokens).
		/// If no value is provided it will default to <see cref="Authority"/> with "protocol/openid-connect/token" appended.
		/// </summary>
		public string TokenUrl {
			get => string.IsNullOrEmpty(_tokenUrl) ? Authority + "protocol/openid-connect/token" : _tokenUrl;
			set => _tokenUrl = value;
		}

		/// <summary>
		/// The OpenID Connect authorization url (e.g. used for authorization code flow).
		/// If no value is provided it will default to <see cref="Authority"/> with "protocol/openid-connect/auth" appended.
		/// </summary>
		public string AuthorizationUrl {
			get => string.IsNullOrEmpty(_authorizationUrl) ? Authority + "protocol/openid-connect/auth" : _authorizationUrl;
			set => _authorizationUrl = value;
		}

		/// <summary>
		/// The audience (aka client) we trust.
		/// </summary>
		public string Audience { get; set; } = "";

		/// <summary>
		/// The scopes which need to be present at the token.
		/// All of them are required (aka AND).
		/// </summary>
		public string[] Scopes { get; set; } = Array.Empty<string>();

		/// <summary>
		/// Id of the client used for accessing the API (e.g. at client credentials flow) .
		/// </summary>
		public string ClientId { get; set; } = "";
	}
}
