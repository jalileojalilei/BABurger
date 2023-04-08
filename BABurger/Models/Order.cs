using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BABurger.Models
{

    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Size { get; set; }

        public string? Extra { get; set; }

        public int Number { get; set; }

        public double Price { get; set; }

        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}
