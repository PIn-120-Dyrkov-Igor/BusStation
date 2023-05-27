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
    public class StopsListsController : Controller
    {
        private readonly CourseDBContext _context;

        public StopsListsController(CourseDBContext context)
        {
            _context = context;
        }

        // GET: StopsLists
        public async Task<IActionResult> Index()
        {
            var courseDBContext = _context.StopsLists.Include(s => s.Route).Include(s => s.Stop);
            return View(await courseDBContext.ToListAsync());
        }

        // GET: StopsLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StopsLists == null)
            {
                return NotFound();
            }

            var stopsList = await _context.StopsLists
                .Include(s => s.Route)
                .Include(s => s.Stop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopsList == null)
            {
                return NotFound();
            }

            return View(stopsList);
        }

        // GET: StopsLists/Create
        public IActionResult Create()
        {
            var routes = _context.Routes.ToList();//Custom ViewData Route from-to
            var routesList = routes.Select(route => new SelectListItem
            {
                Value = route.Id.ToString(),
                Text = $"{route.RouteNumber} {route.DepertureCity} - {route.ArrivalCity}"
            });

            var stops = _context.Stops.ToList();//Custom ViewData Stop name
            var stopList = stops.Select(stop => new SelectListItem
            {
                Value = stop.Id.ToString(),
                Text = $"{stop.StopName} ({stop.StopCity})"
            });

            ViewData["RouteId"] = new SelectList(routesList, "Value", "Text");
            ViewData["StopId"] = new SelectList(stopList, "Value", "Text");
            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id");
            //ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id");
            return View();
        }

        // POST: StopsLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RouteId,StopId")] StopsList stopsList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stopsList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopsList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopsList.StopId);
            return View(stopsList);
        }

        // GET: StopsLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StopsLists == null)
            {
                return NotFound();
            }

            var stopsList = await _context.StopsLists.FindAsync(id);
            if (stopsList == null)
            {
                return NotFound();
            }

            var routes = _context.Routes.ToList();//Custom ViewData Route from-to
            var routesList = routes.Select(route => new SelectListItem
            {
                Value = route.Id.ToString(),
                Text = $"{route.RouteNumber} {route.DepertureCity} - {route.ArrivalCity}"
            });

            var stops = _context.Stops.ToList();//Custom ViewData Stop name
            var stopList = stops.Select(stop => new SelectListItem
            {
                Value = stop.Id.ToString(),
                Text = $"{stop.StopName} ({stop.StopCity})"
            });

            ViewData["RouteId"] = new SelectList(routesList, "Value", "Text");
            ViewData["StopId"] = new SelectList(stopList, "Value", "Text");
            /*ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopsList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopsList.StopId);*/
            return View(stopsList);
        }

        // POST: StopsLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RouteId,StopId")] StopsList stopsList)
        {
            if (id != stopsList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stopsList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StopsListExists(stopsList.Id))
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", stopsList.RouteId);
            ViewData["StopId"] = new SelectList(_context.Stops, "Id", "Id", stopsList.StopId);
            return View(stopsList);
        }

        // GET: StopsLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StopsLists == null)
            {
                return NotFound();
            }

            var stopsList = await _context.StopsLists
                .Include(s => s.Route)
                .Include(s => s.Stop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopsList == null)
            {
                return NotFound();
            }

            return View(stopsList);
        }

        // POST: StopsLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StopsLists == null)
            {
                return Problem("Entity set 'CourseDBContext.StopsLists'  is null.");
            }
            var stopsList = await _context.StopsLists.FindAsync(id);
            if (stopsList != null)
            {
                _context.StopsLists.Remove(stopsList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StopsListExists(int id)
        {
          return (_context.StopsLists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
