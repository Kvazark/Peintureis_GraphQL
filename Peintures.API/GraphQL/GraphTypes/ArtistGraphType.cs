using GraphQL.Types;
using Peintures.Core.Entities;

namespace Peintures.GraphQL.GraphTypes
{
    public class ArtistGraphType : ObjectGraphType<Artist>
    {
        public ArtistGraphType()
        {
            Name = "artist";
            Field(a => a.FullNameArtist).Description("The first name and second name of this artist");
            Field(a => a.YearsOfLife);
        }
    }
}