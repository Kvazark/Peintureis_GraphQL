using System.ComponentModel.DataAnnotations;

namespace Peintures.Core.Entities
{
    public class Painting
    {
        [Key] 
        public string Name { get; set; }
        
        public string Artist { get; set; }
       
        public string Owner { get; set; }
        
        public int YearOfPainting { get; set; }
       
        public string Genre { get; set; }
       
        
        public virtual Artist ArtistName { get; set; }
        public virtual Owner OwnerName { get; set; }
    }
}