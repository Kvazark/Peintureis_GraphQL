using GraphQL.Types;
using Peintures.Core.Entities;

namespace Peintures.GraphQL.GraphTypes
{
    public class PaintingGraphType : ObjectGraphType<Painting>
    {
        public PaintingGraphType()
        {
            Name = "Painting";
            Field(p => p.Name, true).Description("Name of painting)");
            Field(c => c.ArtistName, nullable: false, type: typeof(ArtistGraphType))
                .Description("The artist of this painting.");
            Field(c => c.OwnerName, nullable: false, type: typeof(OwnerGraphType))
                .Description("The owner of this painting.");
            Field(p => p.YearOfPainting);
            Field(p => p.Genre);
        }

    }
}