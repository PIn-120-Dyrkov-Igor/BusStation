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
    public class TripsController : Controller
    {
        private readonly CourseDBContext _context;

        public TripsController(CourseDBContext context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var courseDBContext = _context.Trips.Include(t => t.Bus).Include(t => t.DriversComposition).Include(t => t.Route);
            return View(await courseDBContext.ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.DriversComposition)
                .Include(t => t.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            var routes = _context.Routes.ToList();//Custom ViewData Route from-to
            var routesList = routes.Select(route => new SelectListItem
            {
                Value = route.Id.ToString(),
                Text = $"{route.RouteNumber} {route.DepertureCity} - {route.ArrivalCity}"
            });
            ViewData["RouteId"] = new SelectList(routesList, "Value", "Text");

            ViewData["BusId"] = new SelectList(_context.Buses, "Id", "BusName");
            ViewData["DriversCompositionId"] = new SelectList(_context.DriversCompositions, "Id", "Id");
            //ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BusId,RouteId,DriversCompositionId,FreeSeatCount,TripDate,TripTime,TripDateArrival,TripTimeArrival")] Trip trip)
        {
            //Edit for test
            var travelTime = _context.Routes.Where(r => r.Id == trip.RouteId).Select(r => r.TravelTime).FirstOrDefault();//Получаем время в пути
            trip.TripTimeArrival = trip.TripTime + travelTime.TimeOfDay;//Время прибытия = время начала движения + время в пути
            int fr = trip.TripTime.Hour;//время начала движения
            int to = trip.TripTimeArrival.Hour;//время прибытия
            if (fr>to) trip.TripDateArrival = trip.TripDate.AddDays(1);//Если (время начала движения > время прибытия)
            else trip.TripDateArrival = trip.TripDate;
            trip.FreeSeatCount = _context.Buses.Where(r => r.Id == trip.BusId).Select(r => r.SeatNumber).FirstOrDefault();//Количество мест в автобусе


            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();//

                for(int place = 1; place <= trip.FreeSeatCount; place++)
                {
                    Ticket ticket = new Ticket
                    {
                        Trip = trip,
                        SeatNumber = place
                    };
                    _context.Tickets.Add(ticket);
                }//

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BusId"] = new SelectList(_context.Buses, "Id", "Id", trip.BusId);
            ViewData["DriversCompositionId"] = new SelectList(_context.DriversCompositions, "Id", "Id", trip.DriversCompositionId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", trip.RouteId);
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            ViewData["BusId"] = new SelectList(_context.Buses, "Id", "Id", trip.BusId);
            ViewData["DriversCompositionId"] = new SelectList(_context.DriversCompositions, "Id", "Id", trip.DriversCompositionId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", trip.RouteId);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BusId,RouteId,DriversCompositionId,FreeSeatCount,TripDate,TripTime,TripDateArrival,TripTimeArrival")] Trip trip)
        {
            if (id != trip.Id)
            {
                return NotFound();
            }

            //Edit for test
            var travelTime = _context.Routes.Where(r => r.Id == trip.RouteId).Select(r => r.TravelTime).FirstOrDefault();//Получаем время в пути
            trip.TripTimeArrival = trip.TripTime + travelTime.TimeOfDay;//Время прибытия = время начала движения + время в пути
            int fr = trip.TripTime.Hour;//время начала движения
            int to = trip.TripTimeArrival.Hour;//время прибытия
            if (fr > to) trip.TripDateArrival = trip.TripDate.AddDays(1);//Если (время начала движения > время прибытия)
            else trip.TripDateArrival = trip.TripDate;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.Id))
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
            ViewData["BusId"] = new SelectList(_context.Buses, "Id", "Id", trip.BusId);
            ViewData["DriversCompositionId"] = new SelectList(_context.DriversCompositions, "Id", "Id", trip.DriversCompositionId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", trip.RouteId);
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.DriversComposition)
                .Include(t => t.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trips == null)
            {
                return Problem("Entity set 'CourseDBContext.Trips'  is null.");
            }
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
          return (_context.Trips?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
