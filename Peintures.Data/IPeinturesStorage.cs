using System.Collections.Generic;
using Peintures.Core.Entities;

namespace Peintures.Data
{
    public interface IPeinturesStorage
    {
        public int CountPaintings();
        public IEnumerable<Painting> ListPaintings();
        public IEnumerable<Artist> ListArtists();
        public IEnumerable<Owner> ListOwners();

        public Painting FindPainting(string name);
        public Artist FindArtist(string fullNameArtist);
        public Owner FindOwner(string nameOwner);

        public void CreatePainting(Painting painting);
        public void UpdatePainting(Painting painting);
        public void DeletePainting(Painting painting);
    }
}