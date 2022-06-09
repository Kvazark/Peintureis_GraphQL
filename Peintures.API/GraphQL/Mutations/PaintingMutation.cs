using GraphQL;
using GraphQL.Types;
using Peintures.Core.Entities;
using Peintures.Data;
using Peintures.GraphQL.GraphTypes;

namespace Peintures.GraphQL.Mutations
{
    public class PaintingMutation: ObjectGraphType
    {
        private readonly IPeinturesStorage _db;

        public PaintingMutation(IPeinturesStorage db)
        {
            this._db = db;

            Field<PaintingGraphType>(
                "createPainting",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "name"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "artist"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "owner"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "yearOfPainting"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "genre"}

                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var artist = context.GetArgument<string>("artist");
                    var owner = context.GetArgument<string>("owner");
                    var yearOfPainting = context.GetArgument<int>("yearOfPainting");
                    var genre = context.GetArgument<string>("genre");

                    var artistName = db.FindArtist(artist);
                    var ownerName = db.FindOwner(owner);
                    var painting = new Painting
                    {
                        Name = name,
                        ArtistName = artistName,
                        Artist = artistName.FullNameArtist,
                        OwnerName = ownerName,
                        Owner = ownerName.NameOwner,
                        YearOfPainting = yearOfPainting,
                        Genre = genre
                    };
                    
                    _db.CreatePainting(painting);
                    return painting;
                }
            );
        }
    }
}