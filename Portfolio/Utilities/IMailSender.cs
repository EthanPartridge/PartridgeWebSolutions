using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Utilities
{
    public interface IMailSender
    {
        void SendPlainTextEmailSendgrid(string recipientEmail, string recipientName, string clientName, string clientEmail, string clientMessage);
        
        //Can implement these methods if desired
        /*
        void SendPlainTextEmail(string recipientEmail, string recipientName, string clientName, string clientEmail, string clientMessage);
        void SendHtmlEmail(string recipientEmail, string recipientName);
        void SendHtmlWithAttachmentEmail(string recipientEmail, string recipientName);
        void SendHtmlEmailSendGrid(string recipientEmail, string recipientName);
        void SendHtmlWithAttachmentEmailSendgrid(string recipientEmail, string recipientName);
        void SendBulkEmailSendgrid(IEnumerable<string> recipientEmails);
        */
    }
}
