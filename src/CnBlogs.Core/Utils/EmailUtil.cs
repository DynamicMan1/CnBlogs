using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public class EmailUtil : IEmailUtil
    {
        private string host;        // smtp 服务器名称
        private string sender;      // 发送人邮箱
        private string password;    // 发送人邮箱密码

        public EmailUtil(IEmailUtilSettings emailUtilSettings)
        {
            sender = emailUtilSettings.Sender;
            host = emailUtilSettings.Host;
            password = emailUtilSettings.Password;
        }

        public void Send(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(sender));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(host, 25, false);
                client.Authenticate(sender, password);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
