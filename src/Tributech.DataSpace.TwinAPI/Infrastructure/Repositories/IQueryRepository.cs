using System.Threading.Tasks;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Infrastructure.Repository {
	public interface IQueryRepository {
		Task<TwinGraph> GetByCypherQuery(TwinCypherQuery cypherQuery);
		public Task<TwinGraph> GetSubgraph(TwinGraphQuery query);
	}
}
