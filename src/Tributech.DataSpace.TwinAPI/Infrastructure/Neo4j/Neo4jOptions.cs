namespace Tributech.DataSpace.TwinAPI.Infrastructure.Neo4j
{
    public class Neo4jOptions
    {
        public string Host { get; set; } = "neo4j://localhost:7687";
		public string User { get; set; } = "neo4j";
		public string Password { get; set; } = "neo4j";
	}
}
