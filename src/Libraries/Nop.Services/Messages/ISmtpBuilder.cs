using MailKit.Net.Smtp;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    public interface ISmtpBuilder
    {
        /// <summary>
        /// Create a new SMTP client for a default email account
        /// </summary>
        /// <returns>An SMTP client that can be used to send email messages</returns>
        SmtpClient Build();

        /// <summary>
        /// Create a new SMTP client for a specific email account
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <returns>An SMTP client that can be used to send email messages</returns>
        SmtpClient Build(EmailAccount emailAccount);
    }
}
