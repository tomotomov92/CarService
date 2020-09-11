using System.Net.Mail;

namespace BusinessLogic.EmailSender
{
    public class EmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _fromAddress;

        public EmailSender(SmtpClient smtpClient, MailAddress fromAddress)
        {
            _smtpClient = smtpClient;
            _fromAddress = fromAddress;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var toAddress = new MailAddress(to);

            using (var message = new MailMessage(_fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                _smtpClient.Send(message);
            }
        }
    }
}
