using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BABurger.Models
{
    [Table("Topping")]
    public class Topping
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
    }
}
