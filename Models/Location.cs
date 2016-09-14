using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RPG.Models
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
