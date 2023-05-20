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
            var courseDBContext = _context.StopLists.Include(s => s.Route);
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
            var stops = _context.Stops.ToList();//Custom ViewData Stop name
            var stopList = stops.Select(stop => new SelectListItem
            {
                Value = stop.Id.ToString(),
                Text = $"{stop.StopName} ({stop.StopCity})"
            });
            ViewData["StopId"] = new SelectList(stopList, "Value", "Text");

            var routes = _context.Routes.ToList();//Custom ViewData Route from-to
            var routesList = routes.Select(route => new SelectListItem
            {
                Value = route.Id.ToString(),
                Text = $"{route.RouteNumber} {route.DepertureCity} - {route.ArrivalCity}"
            });
            ViewData["RouteId"] = new SelectList(routesList, "Value", "Text");
            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteNumber");
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteNumber", stopList.RouteId);
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

            var stops = _context.Stops.ToList();//Custom ViewData Stop name
            var stopLists = stops.Select(stop => new SelectListItem
            {
                Value = stop.Id.ToString(),
                Text = $"{stop.StopName} ({stop.StopCity})"
            });
            ViewData["StopId"] = new SelectList(stopLists, "Value", "Text");

            var routes = _context.Routes.ToList();//Custom ViewData Route from-to
            var routesList = routes.Select(route => new SelectListItem
            {
                Value = route.Id.ToString(),
                Text = $"{route.RouteNumber} {route.DepertureCity} - {route.ArrivalCity}"
            });
            ViewData["RouteId"] = new SelectList(routesList, "Value", "Text");
            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteNumber", stopList.RouteId);
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "RouteNumber", stopList.RouteId);
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
