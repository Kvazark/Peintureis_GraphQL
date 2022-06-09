using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using Peintures.Core.Entities;
using Peintures.Data;
using Peintures.GraphQL.GraphTypes;

namespace Peintures.GraphQL.Queries
{
    public class PaintingQuery: ObjectGraphType
    {
        private readonly IPeinturesStorage _db;

        public PaintingQuery(IPeinturesStorage db)
        {
            _db = db;

            Field<ListGraphType<PaintingGraphType>>("Paintings", "Query to retrieve all Paintings",
                resolve: GetAllPaintings);

            Field<PaintingGraphType>("Painting", "Query to retrieve a specific painting",
                new QueryArguments(MakeNonNullStringArgument("name", "Name painting")),
                resolve: GetPainting);

            Field<ListGraphType<PaintingGraphType>>("PaintingsByGenre", "Query to retrieve all Paintings the specified genre",
                new QueryArguments(MakeNonNullStringArgument("genre", "The name of a genre")),
                resolve: GetPaintingsByGenre);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description)
        {
            return new QueryArgument<NonNullGraphType<StringGraphType>>
            {
                Name = name, Description = description
            };
        }

        private IEnumerable<Painting> GetAllPaintings(IResolveFieldContext<object> context) => _db.ListPaintings();

        private Painting GetPainting(IResolveFieldContext<object> context)
        {
            var name = context.GetArgument<string>("name");
            return _db.FindPainting(name);
        }

        private IEnumerable<Painting> GetPaintingsByGenre(IResolveFieldContext<object> context)
        {
            var genre = context.GetArgument<string>("genre");
            var paintings = _db.ListPaintings()
                .Where(p => p.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase));
            return paintings;
        }

       
    }
}