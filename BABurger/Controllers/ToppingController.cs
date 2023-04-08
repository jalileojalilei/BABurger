﻿using System;
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

namespace BABurger.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ToppingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToppingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Topping
        public async Task<IActionResult> Index()
        {
              return _context.Toppings != null ? 
                          View(await _context.Toppings.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Toppings'  is null.");
        }

        // GET: Topping/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Toppings == null)
            {
                return NotFound();
            }

            var topping = await _context.Toppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topping == null)
            {
                return NotFound();
            }

            return View(topping);
        }

        // GET: Topping/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Topping topping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topping);
        }

        // GET: Topping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Toppings == null)
            {
                return NotFound();
            }

            var topping = await _context.Toppings.FindAsync(id);
            if (topping == null)
            {
                return NotFound();
            }
            return View(topping);
        }

        // POST: Topping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Topping topping)
        {
            if (id != topping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToppingExists(topping.Id))
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
            return View(topping);
        }

        // GET: Topping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Toppings == null)
            {
                return NotFound();
            }

            var topping = await _context.Toppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topping == null)
            {
                return NotFound();
            }

            return View(topping);
        }

        // POST: Topping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Toppings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Toppings'  is null.");
            }
            var topping = await _context.Toppings.FindAsync(id);
            if (topping != null)
            {
                _context.Toppings.Remove(topping);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToppingExists(int id)
        {
          return (_context.Toppings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
