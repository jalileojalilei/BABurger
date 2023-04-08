using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BABurger.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
    }
}
