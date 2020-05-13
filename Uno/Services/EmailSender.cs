using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Uno.Entities;

namespace Uno.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713

   //per implementare l'invio mail riferirsi a:
   //https://kenhaggerty.com/articles/article/aspnet-core-22-smtp-emailsender-implementation
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;

        public EmailSender(IOptions<EmailSettings> emailSettings,
        IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(_emailSettings.Sender);
            msg.To.Add(new MailAddress(email));
            msg.Subject = subject;

            #region formatter
            string text = string.Format("Please click on this link to {0}: {1}", subject, message);

            //string html = "Please confirm your account by clicking this link: <a href='" + message.Body + "'>link</a><br/>";
            string html = message;
            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message);
            #endregion
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            //impostare Less secure app access enabled se si vuole usare l'smtp di google per spedire messaggi, andare qui per impostarlo:
            //https://myaccount.google.com/lesssecureapps
            SmtpClient smtpClient = new SmtpClient(_emailSettings.MailServer, Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password.ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);


            return Task.CompletedTask;
        }
    }
}
