using Microsoft.AspNetCore.Mvc.Rendering;

namespace BABurger.Models
{
    public class OrdersViewModel
    {
        public Order? Order { get; set; }
        public int? SelectedMenuId { get; set; }
        public SizeEnum? SelectedSize { get; set; }
        public IEnumerable<SelectListItem> MenuItems { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SizeItems { get; set; } = new List<SelectListItem>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();
    }
}
