using GraphQL.Types;
using Peintures.Data;
using Peintures.GraphQL.Mutations;
using Peintures.GraphQL.Queries;

namespace Peintures.GraphQL.Schemas
{
    public class PeinturesSchema: Schema {
        public PeinturesSchema(IPeinturesStorage db)
        {
            Query = new PaintingQuery(db);
            Mutation = new PaintingMutation(db);
        }
    }
}