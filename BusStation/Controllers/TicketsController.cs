using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusStation.Models;
using System.Net.Sockets;

namespace BusStation.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CourseDBContext _context;

        public TicketsController(CourseDBContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            /*var courseDBContext = _context.Tickets.Include(t => t.Passanger).Include(t => t.Route).Include(t => t.Trip);
            return View(await courseDBContext.ToListAsync());*/
            return View(await _context.Passangers.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(int passangerId)
        {
            Passanger passanger = await _context.Passangers.FindAsync(passangerId);
            if (passanger == null)
                return NotFound();

            ViewBag.PassangerId = passangerId;
            ViewBag.Passanger = passanger;
            return View("ChooseDate");
        }

        [HttpPost]
        public async Task<IActionResult> ChooseDate(int passangerId, DateTime sessionDate)//Выбор направления(Переход)
        {
            Passanger passanger = await _context.Passangers.FindAsync(passangerId);
          
            ViewBag.PassangerId = passangerId;
            ViewBag.sessionDate = sessionDate.ToLongDateString();
            ViewBag.Passanger = passanger.Name;
            /*return View("ChooseRoute", await _context.Routes.ToListAsync());*/
            return View("ChooseRoute", await _context.Routes.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ChooseRoute(int passangerId, DateTime sessionDate, int routeId)//Выбор рейса(Переход)
        {
            Passanger passanger = await _context.Passangers.FindAsync(passangerId);

            var Trips = await _context.Trips
                .Where(t => t.RouteId == routeId)
                .Where(m => m.TripDate.Date == sessionDate.Date)
                .Where(m => m.TripTime.Hour > DateTime.Now.Hour || (m.TripTime.Hour == DateTime.Now.Hour && m.TripTime.Minute > DateTime.Now.Minute)).ToListAsync();

            ViewBag.PassangerId = passangerId;
            ViewBag.sessionDate = sessionDate.ToLongDateString();
            ViewBag.Passanger = passanger.Name;
            ViewBag.Route = routeId;
            return View("ChooseTrip", Trips);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseTrip(int passangerId, DateTime sessionDate, int routeId, int tripId)//Выбор остановки(Переход)
        {
            Passanger passanger = await _context.Passangers.FindAsync(passangerId);

            var StopsLists = await _context.StopsLists
                .Include(t => t.Route)
                .Include(t => t.Stop)
                .Where(t => t.RouteId == routeId)
                /*.Where(m => m.TripDate.Date == sessionDate.Date)*/.ToListAsync();

            ViewBag.PassangerId = passangerId;
            ViewBag.sessionDate = sessionDate.ToLongDateString();
            ViewBag.Passanger = passanger.Name;
            ViewBag.Route = routeId;
            ViewBag.Trip = tripId;
            return View("ChooseStop", StopsLists);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseStop(int passangerId, DateTime sessionDate, int routeId, int tripId, int stopId)
        {
            Passanger passanger = await _context.Passangers.FindAsync(passangerId);

            List<Ticket> tickets = _context.Tickets
                .Include(t => t.Route)
                .Include(t => t.Route)
                .ToList();



            ViewBag.PassangerId = passangerId;
            ViewBag.sessionDate = sessionDate.ToLongDateString();
            ViewBag.Passanger = passanger;
            ViewBag.Route = routeId;
            ViewBag.Trip = tripId;
            ViewBag.Stop = stopId;

            int seatNumber = _context.Trips//Получаем количество мест автобуса
                .Where(t => t.Id == tripId)
                .Select(t => t.Bus.SeatNumber)
                .FirstOrDefault();

            bool[] seats = new bool[seatNumber];

            foreach (Ticket ticket in _context.Tickets.Where(t => t.TripId == tripId))
            {
                seats[seatNumber - 1] = ticket.PassangerId == null;
            }

            ViewBag.SeatCount = seatNumber;
            ViewBag.Seats = seats;

            return View("ChooseTicket", tickets);
        }

        [HttpPost]
        public ActionResult ChooseTicket(int passangerId, DateTime sessionDate, int routeId, int tripId, int stopId, int SeatNumber)
        {
            Passanger passanger = _context.Passangers.Find(passangerId);
            Ticket ticket = _context.Tickets.FirstOrDefault(t => t.SeatNumber == SeatNumber &&  t.RouteId == routeId);
            Models.Route route = _context.Routes
                                    .Include(m => m.DepertureCity)
                                    .Include(m => m.ArrivalCity)
                                    .FirstOrDefault(m => m.Id == routeId);

            ViewBag.PassangerId = passangerId;
            ViewBag.Passanger = passanger;
            ViewBag.Route = routeId;
            ViewBag.Trip = tripId;
            ViewBag.Ticket = ticket;
            ViewBag.RouteModel = route;

            return View("ComfimBuy");
        }

        [HttpPost]
        public IActionResult ComfimBuy(int passangerId, int routeId, int tripId, int ticketId)
        {
            Ticket ticket = _context.Tickets.Find(ticketId);
            ticket.TicketNumber = _context.Tickets.OrderByDescending(t => t.TicketNumber).FirstOrDefault() != null ? ticket.TicketNumber : 1;
            ticket.DateSale = DateTime.Now;
            ticket.RouteId = routeId;
            ticket.TripId = tripId;
            ticket.PassangerId = passangerId;

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Passanger)
                .Include(t => t.Route)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["PassangerId"] = new SelectList(_context.Passangers, "Id", "Id");
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketNumber,DateSale,RouteId,TripId,PassangerId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PassangerId"] = new SelectList(_context.Passangers, "Id", "Id", ticket.PassangerId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", ticket.RouteId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", ticket.TripId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["PassangerId"] = new SelectList(_context.Passangers, "Id", "Id", ticket.PassangerId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", ticket.RouteId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", ticket.TripId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketNumber,DateSale,RouteId,TripId,PassangerId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["PassangerId"] = new SelectList(_context.Passangers, "Id", "Id", ticket.PassangerId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "Id", ticket.RouteId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Id", ticket.TripId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Passanger)
                .Include(t => t.Route)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'CourseDBContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        // GET: Tickets //After multiple actions controllers 
        public async Task<IActionResult> IndexAdmin()
        {
            var courseDBContext = _context.Tickets.Include(t => t.Passanger).Include(t => t.Route).Include(t => t.Trip);
            return View(await courseDBContext.ToListAsync());
        }
    }
}
