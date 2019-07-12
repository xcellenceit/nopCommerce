using System;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Default SMTP Generator
    /// </summary>
    public class DefaultSmtpBuilder : ISmtpBuilder
    {
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEmailAccountService _emailAccountService;

        public DefaultSmtpBuilder(EmailAccountSettings emailAccountSettings, IEmailAccountService emailAccountService)
        {
            _emailAccountSettings = emailAccountSettings;
            _emailAccountService = emailAccountService;
        }

        /// <summary>
        /// Create a new SMTP client for a default email account
        /// </summary>
        /// <returns>An SMTP client that can be used to send email messages</returns>
        public virtual SmtpClient Build()
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId) 
                ?? throw new NopException("Email account could not be loaded");

            var client = new SmtpClient();

            try
            {
                ConfigureClient(client, emailAccount);
                return client;
            }
            catch (Exception ex)
            {
                client.Dispose();
                throw new NopException(ex.Message);
            }
        }

        /// <summary>
        /// Create a new SMTP client for a specific email account
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <returns>An SMTP client that can be used to send email messages</returns>
        public virtual SmtpClient Build(EmailAccount emailAccount)
        {
            var client = new SmtpClient();

            try
            {
                ConfigureClient(client, emailAccount);
                return client;
            }
            catch (Exception ex)
            {
                client.Dispose();
                throw new NopException(ex.Message);
            }
        }

        /// <summary>
        /// Configuring the SMTP client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="emailAccount"></param>
        protected virtual void ConfigureClient(SmtpClient client, EmailAccount emailAccount)
        {
            client.Connect(
                emailAccount.Host,
                emailAccount.Port,
                emailAccount.EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable
            );

            client.Authenticate(emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password));
        }
    }
}