using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FeedbackSchool.Models
{
    public class SendMail : IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("descomproject2020@gmail.com", "Администрация");
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to)
            {
                // тема письма
                Subject = subject, 
                // текст письма
                Body = htmlMessage, 
                // письмо представляет код html
                IsBodyHtml = true
            };
            
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("login", "pass"),
                EnableSsl = true
            };
            // логин и пароль
            smtp.Send(m);

            return Task.CompletedTask;
        }
    }
}