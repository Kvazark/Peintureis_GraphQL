using GraphQL.Types;
using Peintures.Core.Entities;

namespace Peintures.GraphQL.GraphTypes
{
    public class OwnerGraphType: ObjectGraphType<Owner> 
    {
        public OwnerGraphType()
        {
            Name = "owner";
            Field(o => o.NameOwner).Description("The name of this owner");
            Field(o => o.Type);
            Field(o => o.Location);
        }
    }
}