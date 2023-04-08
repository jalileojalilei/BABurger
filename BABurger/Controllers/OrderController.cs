using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BABurger.Data;
using BABurger.Models;
using BABurger.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BABurger.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
              return _context.Orders != null ? 
                          View(await _context.Orders.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var menus = _context.Menus.ToList();
            var menuSelectList = new SelectList(menus, "Id", "Name");

            var sizeSelectList = Enum.GetValues(typeof(SizeEnum))
                             .Cast<SizeEnum>()
                             .Select(s => new SelectListItem
                             {
                                 Value = s.ToString(),
                                 Text = s.ToString()
                             });

            var toppings = _context.Toppings.ToList(); // Get the list of toppings from the database

            var ordersViewModel = new OrdersViewModel
            {
                Order = new Order(),
                MenuItems = menuSelectList,
                SizeItems = sizeSelectList,
                Toppings = toppings,

            };

            return View(ordersViewModel);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdersViewModel ordersViewModel, List<int> toppingNumbers)
        {
            var menus = _context.Menus.ToList();
            var menuSelectList = new SelectList(menus, "Id", "Name");
            ordersViewModel.MenuItems = menuSelectList;

            if (ModelState.IsValid)
            {
                // Set the Name property based on the selected menu item
                var selectedMenu = _context.Menus.FirstOrDefault(m => m.Id == ordersViewModel.SelectedMenuId);
                if (selectedMenu != null)
                {
                    ordersViewModel.Order.Name = selectedMenu.Name;
                }

                // Set the Size property based on the selected size item
                ordersViewModel.Order.Size = ordersViewModel.SelectedSize.ToString();

                // Create a dictionary to hold the topping quantities
                var toppingQuantities = new Dictionary<string, int>();

                // Loop through the toppings and their corresponding numbers
                foreach (var topping in ordersViewModel.Toppings)
                {
                    var toppingNumber = toppingNumbers.FirstOrDefault(tn => tn == topping.Id);
                    if (toppingNumber > 0)
                    {
                        toppingQuantities.Add(topping.Name, toppingNumber);
                    }
                }

                // Convert the dictionary to a formatted string
                var toppingsString = string.Join(", ", toppingQuantities.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

                // Set the Extra property to the toppings string
                ordersViewModel.Order.Extra = toppingsString;

                _context.Add(ordersViewModel.Order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordersViewModel);
        }





        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Size,Extra,Number,Price,OrderTime")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
