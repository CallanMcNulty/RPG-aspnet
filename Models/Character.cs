using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Models
{
    [Table("Characters")]
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int LocationId { get; set; }
        public virtual ICollection<Item> Inventory { get; set; }
    }
}
