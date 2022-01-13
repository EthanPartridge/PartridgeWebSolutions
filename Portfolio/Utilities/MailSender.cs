using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Portfolio.Utilities
{
    public class MailSender : IMailSender
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public MailSender(IServiceProvider serviceProvider, ILogger<MailSender> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        //Sends emails when users fill out contact form
        public async void SendPlainTextEmailSendgrid(string recipientEmail, string recipientName, string clientName, string clientEmail, string clientMessage)
        {
            var body = new StringBuilder();
            body.AppendLine("Name: " + clientName)
                .AppendLine("Email: " + clientEmail)
                .AppendLine("")
                .AppendLine("Message: ")
                .AppendLine(clientMessage);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mailSettings = scope.ServiceProvider.GetRequiredService<IFluentEmail>();
                    var email = mailSettings
                        .To(recipientEmail, recipientName)
                        .Subject("Message From: " + clientName)
                        .Body(body.ToString());

                    await email.SendAsync();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                _logger.LogError("Email Sent = [Name: " + clientName + "], "
                    + "[Email: " + clientEmail + "], "
                    + "[Message: " + clientMessage + "]");
            }
            finally
            {
                _logger.LogInformation("Email Sent = [Name: " + clientName + "], "
                        + "[Email: " + clientEmail + "], "
                        + "[Message: " + clientMessage + "]");
            }
        }

        /*
        public async void SendPlainTextEmail(string recipientEmail, string recipientName, string clientEmail, string clientName, string clientMessage)
        {
            throw new NotImplementedException();
        }

        public void SendHtmlEmail(string recipientEmail, string recipientName)
        {
            throw new NotImplementedException();
        }

        public void SendHtmlWithAttachmentEmail(string recipientEmail, string recipientName)
        {
            throw new NotImplementedException();
        }

        

        public void SendHtmlEmailSendGrid(string recipientEmail, string recipientName)
        {
            throw new NotImplementedException();
        }
        
        public void SendHtmlWithAttachmentEmailSendgrid(string recipientEmail, string recipientName)
        {
            throw new NotImplementedException();
        }
       
        public void SendBulkEmailSendgrid(IEnumerable<string> recipientEmails)
        {
            throw new NotImplementedException();
        }
        */
    }
}
