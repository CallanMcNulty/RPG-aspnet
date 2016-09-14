using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Models
{
    [Table("Items")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}