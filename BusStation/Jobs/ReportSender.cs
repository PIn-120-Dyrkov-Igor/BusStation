using System.Net.Mail;
using System.Net;
using BusStation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using OfficeOpenXml;
using Quartz;
using System;


namespace BusStation.Jobs
{
    public class ReportSender : IJob
    {
        string file_path_template;
        string file_path_report;
        private readonly CourseDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public ReportSender(CourseDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public void PrepareReport()
        {
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(file_path_template))
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
                //сохраняем в новое место
                excelPackage.SaveAs(file_path_report);
            }
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/passangersSample.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/passangersReport.xlsx";
            file_path_template = _appEnvironment.WebRootPath + path;
            file_path_report = _appEnvironment.WebRootPath + result;
            try
            {
                if (File.Exists(file_path_report))
                    File.Delete(file_path_report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            PrepareReport();
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("hell-resster@mail.ru", "BusStation");
            // кому отправляем
            //MailAddress to = new MailAddress("zdixd@mail.ru");//TO send
            MailAddress to = new MailAddress("hell-resster@mail.ru");//TO send
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Еженедельное оповещение";
            // текст письма
            m.Body = "<h2>Обновленный список всех пассажиров</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("hell-resster@mail.ru", "RfdersQyiux9yf5t6bt6");
            smtp.EnableSsl = true;
            // вкладываем файл в письмо
            m.Attachments.Add(new Attachment(file_path_report));
            // отправляем асинхронно
            await smtp.SendMailAsync(m);
            m.Dispose();
        }
    }
}
