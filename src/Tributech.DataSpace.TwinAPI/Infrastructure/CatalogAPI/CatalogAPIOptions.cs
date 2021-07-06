namespace Tributech.DataSpace.TwinAPI.Infrastructure.CatalogAPI
{
    public class CatalogAPIOptions
    {
        public string ApiEndpoint { get; set; }
		public string Authority { get; set; }
		public string ClientId { get; set; } = "catalog-api";
		public string ClientSecret { get; set; }
		public string[] Scopes { get; set; }
	}
}
