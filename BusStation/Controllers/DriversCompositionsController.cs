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
    public class DriversCompositionsController : Controller
    {
        private readonly CourseDBContext _context;

        public DriversCompositionsController(CourseDBContext context)
        {
            _context = context;
        }

        // GET: DriversCompositions
        public async Task<IActionResult> Index()
        {
            var courseDBContext = _context.DriversCompositions.Include(d => d.Driver1).Include(d => d.Driver2);
            return View(await courseDBContext.ToListAsync());
        }

        // GET: DriversCompositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DriversCompositions == null)
            {
                return NotFound();
            }

            var driversComposition = await _context.DriversCompositions
                .Include(d => d.Driver1)
                .Include(d => d.Driver2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driversComposition == null)
            {
                return NotFound();
            }

            return View(driversComposition);
        }

        // GET: DriversCompositions/Create
        public IActionResult Create()
        {
            var drivers = _context.Drivers.ToList();//Custom ViewData FIO
            var driversList = drivers.Select(driver => new SelectListItem
            {
                Value = driver.Id.ToString(),
                Text = $"{driver.Surname} {driver.Name[0]}. {driver.Patronymic[0]}."
            });

            ViewData["Driver1Id"] = new SelectList(driversList, "Value", "Text");
            ViewData["Driver2Id"] = new SelectList(driversList, "Value", "Text");
            //ViewData["Driver1Id"] = new SelectList(_context.Drivers, "Id", "Name");
            //ViewData["Driver2Id"] = new SelectList(_context.Drivers, "Id", "Name");
            return View();
        }

        // POST: DriversCompositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Driver1Id,Driver2Id")] DriversComposition driversComposition)
        {
            if (driversComposition.Driver1Id == driversComposition.Driver2Id) ModelState.AddModelError("Driver2Id", "Один водитель не может занимать два места");//Если два одинаковых водителя

            if (ModelState.IsValid)
            {
                _context.Add(driversComposition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var drivers = _context.Drivers.ToList();//Custom ViewData FIO
            var driversList = drivers.Select(driver => new SelectListItem
            {
                Value = driver.Id.ToString(),
                Text = $"{driver.Surname} {driver.Name[0]}. {driver.Patronymic[0]}."
            });

            ViewData["Driver1Id"] = new SelectList(driversList, "Value", "Text");
            ViewData["Driver2Id"] = new SelectList(driversList, "Value", "Text");
            //ViewData["Driver1Id"] = new SelectList(_context.Drivers, "Id", "Name");
            //ViewData["Driver2Id"] = new SelectList(_context.Drivers, "Id", "Name");
            return View(driversComposition);
        }

        // GET: DriversCompositions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DriversCompositions == null)
            {
                return NotFound();
            }

            var driversComposition = await _context.DriversCompositions.FindAsync(id);
            if (driversComposition == null)
            {
                return NotFound();
            }

            var drivers = _context.Drivers.ToList();//Custom ViewData FIO
            var driversList = drivers.Select(driver => new SelectListItem
            {
                Value = driver.Id.ToString(),
                Text = $"{driver.Surname} {driver.Name[0]}. {driver.Patronymic[0]}."
            });

            ViewData["Driver1Id"] = new SelectList(driversList, "Value", "Text", driversComposition.Driver1Id);
            ViewData["Driver2Id"] = new SelectList(driversList, "Value", "Text", driversComposition.Driver2Id);
            //ViewData["Driver1Id"] = new SelectList(_context.Drivers, "Id", "Name", driversComposition.Driver1Id);
            //ViewData["Driver2Id"] = new SelectList(_context.Drivers, "Id", "Name", driversComposition.Driver2Id);
            return View(driversComposition);
        }

        // POST: DriversCompositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Driver1Id,Driver2Id")] DriversComposition driversComposition)
        {
            if (id != driversComposition.Id)
            {
                return NotFound();
            }

            if (driversComposition.Driver1Id == driversComposition.Driver2Id) ModelState.AddModelError("Driver2Id", "Один водитель не может занимать два места");//Если два одинаковых водителя

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driversComposition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriversCompositionExists(driversComposition.Id))
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

            var drivers = _context.Drivers.ToList();//Custom ViewData FIO
            var driversList = drivers.Select(driver => new SelectListItem
            {
                Value = driver.Id.ToString(),
                Text = $"{driver.Surname} {driver.Name[0]}. {driver.Patronymic[0]}."
            });

            ViewData["Driver1Id"] = new SelectList(driversList, "Value", "Text", driversComposition.Driver1Id);
            ViewData["Driver2Id"] = new SelectList(driversList, "Value", "Text", driversComposition.Driver2Id);
            //ViewData["Driver1Id"] = new SelectList(_context.Drivers, "Id", "Name", driversComposition.Driver1Id);
            //ViewData["Driver2Id"] = new SelectList(_context.Drivers, "Id", "Name", driversComposition.Driver2Id);
            return View(driversComposition);
        }

        // GET: DriversCompositions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DriversCompositions == null)
            {
                return NotFound();
            }

            var driversComposition = await _context.DriversCompositions
                .Include(d => d.Driver1)
                .Include(d => d.Driver2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driversComposition == null)
            {
                return NotFound();
            }

            return View(driversComposition);
        }

        // POST: DriversCompositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DriversCompositions == null)
            {
                return Problem("Entity set 'CourseDBContext.DriversCompositions'  is null.");
            }
            var driversComposition = await _context.DriversCompositions.FindAsync(id);
            if (driversComposition != null)
            {
                _context.DriversCompositions.Remove(driversComposition);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriversCompositionExists(int id)
        {
          return (_context.DriversCompositions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
