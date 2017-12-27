using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ProjetoG6.Services;

namespace ProjetoG6.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Email de confirmação",
                $"<br/><br/>A sua conta foi criada com sucesso." + 
                $"<br/><br/> Por favor carregue no Link para ativar a conta: " +
                $"<a href='{HtmlEncoder.Default.Encode(link)}'>Confirmar Contar </a>");
        }
    }
}
