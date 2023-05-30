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
    public class SpamSender : IJob
    {
        string file_path_template;
        string file_path_report;
        private readonly CourseDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public SpamSender(CourseDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var passangers = _context.Passangers;

            foreach (var passenger in passangers)
            {
                if (passenger.Email != null)
                {
                    string email = passenger.Email;

                    // отправитель - устанавливаем адрес и отображаемое в письме имя
                    MailAddress from = new MailAddress("hell-resster@mail.ru", "Имя отправителя");
                    // кому отправляем
                    //MailAddress to = new MailAddress("zdixd@mail.ru");//TO send
                    MailAddress to = new MailAddress(email);//TO send
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Тест rassilki bez fayla";
                    // текст письма
                    m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("hell-resster@mail.ru", "RfdersQyiux9yf5t6bt6");
                    smtp.EnableSsl = true;
                    // отправляем асинхронно
                    await smtp.SendMailAsync(m);
                    m.Dispose();
                }
            }
           
        }
    }
}
