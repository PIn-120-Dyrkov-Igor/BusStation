using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusStation.Models;
using Microsoft.AspNetCore.Identity;
using BusStation.Areas.Identity.Data;
using OfficeOpenXml;//LB6

namespace BusStation.Controllers
{
    public class PassangersController : Controller
    {
        private readonly CourseDBContext _context;
        private readonly UserManager<BusStationUser> _userManager;//For get email user
        private readonly IWebHostEnvironment _appEnvironment;//Lb6

        public PassangersController(CourseDBContext context, UserManager<BusStationUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;//For get email user
            _appEnvironment = appEnvironment;//Lb6
        }

        // GET: Passangers
        public async Task<IActionResult> Index()
        {
              return _context.Passangers != null ? 
                          View(await _context.Passangers.ToListAsync()) :
                          Problem("Entity set 'CourseDBContext.Passangers'  is null.");
        }      

        // GET: Passangers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Passangers == null)
            {
                return NotFound();
            }

            var passanger = await _context.Passangers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passanger == null)
            {
                return NotFound();
            }

            return View(passanger);
        }

        // GET: Passangers/Create
        public IActionResult Create(bool? fromTakeTickets)
        {
            if (fromTakeTickets == true)
                ViewBag.ToTakeTickets = true;
            else
                ViewBag.ToTakeTickets = false;

            return View();
        }
        /*public IActionResult Create()
        {
            return View();
        }*/

        // POST: Passangers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Surname,Name,Patronymic,PassportSeries,PassportNumber,DateOfBird,Email,ToTakeTickets")] Passanger passanger)
        {
            var userIdentity = await _userManager.GetUserAsync(User);
            var pUser = await _userManager.FindByIdAsync(userIdentity.Id);
            passanger.Email = pUser.Email;

            if (ModelState.IsValid)
            {
                _context.Add(passanger);
                await _context.SaveChangesAsync();
                if (passanger.ToTakeTickets == true)
                    return RedirectToAction("Index", "Tickets");
                else
                    return RedirectToAction(nameof(Index));

                /*return RedirectToAction(nameof(Index));*/
            }
            return View(passanger);
        }

        // GET: Passangers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passangers == null)
            {
                return NotFound();
            }

            var passanger = await _context.Passangers.FindAsync(id);
            if (passanger == null)
            {
                return NotFound();
            }
            return View(passanger);
        }

        // POST: Passangers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Surname,Name,Patronymic,PassportSeries,PassportNumber,DateOfBird,Email")] Passanger passanger)
        {
            if (id != passanger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passanger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassangerExists(passanger.Id))
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
            return View(passanger);
        }

        // GET: Passangers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passangers == null)
            {
                return NotFound();
            }

            var passanger = await _context.Passangers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passanger == null)
            {
                return NotFound();
            }

            return View(passanger);
        }

        // POST: Passangers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passangers == null)
            {
                return Problem("Entity set 'CourseDBContext.Passangers'  is null.");
            }
            var passanger = await _context.Passangers.FindAsync(id);
            if (passanger != null)
            {
                _context.Passangers.Remove(passanger);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassangerExists(int id)
        {
          return (_context.Passangers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Lab_6 Add code
        public FileResult GetReport()
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/passangersSample.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/passangersReport.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Дырков И.Д.";
                excelPackage.Workbook.Properties.Title = "Список пассажиров";
                excelPackage.Workbook.Properties.Subject = "Пассажиры";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Лист1"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int startLine = 3;
                List<Passanger> Passangers = _context.Passangers.ToList();
                foreach (Passanger passanger in Passangers)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = passanger.Id;
                    worksheet.Cells[startLine, 3].Value = passanger.Surname;
                    worksheet.Cells[startLine, 4].Value = passanger.Name;
                    worksheet.Cells[startLine, 5].Value = passanger.Patronymic;
                    worksheet.Cells[startLine, 6].Value = passanger.PassportSeries;
                    worksheet.Cells[startLine, 7].Value = passanger.PassportNumber;
                    worksheet.Cells[startLine, 8].Value = passanger.DateOfBird;
                    worksheet.Cells[startLine, 9].Value = passanger.Email;
                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = "passangersReport.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
