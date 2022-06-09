using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Peintures.Core.Entities
{
    public class Owner
    {
        public Owner()
        {
            Paintings = new HashSet<Painting>();
        }
        
        [Key]
        public string NameOwner { set; get; }
       
        public string Type { set; get; }
        public string Location { get; set; }
        
        public virtual ICollection<Painting> Paintings { get; set; }
    }
}