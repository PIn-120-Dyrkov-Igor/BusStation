using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusStation.Models;

namespace BusStation.Controllers
{
    public class StopListsController : Controller
    {
        private readonly CourseDBContext _context;

        public StopListsController(CourseDBContext context)
        {
            _context = context;
        }

        // GET: StopLists
        public async Task<IActionResult> Index()
        {
            var courseDBContext = _context.StopLists.Include(s => s.Route).Include(s => s.Stop);
            return View(await courseDBContext.ToListAsync());
        }

        // GET: StopLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StopLists == null)
            {
                return NotFound();
            }

            var stopList = await _context.StopLists
                .Include(s => s.Route)
                .Include(s => s.Stop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopList == null)
            {
                return NotFound();
            }

            return View(stopList);
        }

        // GET: StopLists/Create
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id");
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id");
            return View();
        }

        // POST: StopLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StopId,RouteId")] StopList stopList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stopList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopList.StopId);
            return View(stopList);
        }

        // GET: StopLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StopLists == null)
            {
                return NotFound();
            }

            var stopList = await _context.StopLists.FindAsync(id);
            if (stopList == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopList.StopId);
            return View(stopList);
        }

        // POST: StopLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StopId,RouteId")] StopList stopList)
        {
            if (id != stopList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stopList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StopListExists(stopList.Id))
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopList.StopId);
            return View(stopList);
        }

        // GET: StopLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StopLists == null)
            {
                return NotFound();
            }

            var stopList = await _context.StopLists
                .Include(s => s.Route)
                .Include(s => s.Stop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopList == null)
            {
                return NotFound();
            }

            return View(stopList);
        }

        // POST: StopLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StopLists == null)
            {
                return Problem("Entity set 'CourseDBContext.StopLists'  is null.");
            }
            var stopList = await _context.StopLists.FindAsync(id);
            if (stopList != null)
            {
                _context.StopLists.Remove(stopList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StopListExists(int id)
        {
          return (_context.StopLists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
