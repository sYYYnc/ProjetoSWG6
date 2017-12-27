using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetoG6.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var fromEmail = new MailAddress("sebastian_tm0@hotmail.com", "CIMOB-SW");
            var toEmail = new MailAddress(email);
            var fromEmailPassord = "Timisoara_02**";
            var smtp = new SmtpClient
            {
                Host = "smtp.live.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassord)
            };
            using (var msg = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })
                smtp.Send(msg);

            return Task.CompletedTask;
        }
    }
}
