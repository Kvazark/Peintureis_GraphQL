using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Peintures.Core.Entities
{
    public class Artist
    {
        public Artist() {
            Paintings = new HashSet<Painting>();
        }
        [Key] 
        public string FullNameArtist { get; set; }
        public string YearsOfLife { get; set; }

        public virtual  ICollection<Painting> Paintings { get; set; }
    }
}