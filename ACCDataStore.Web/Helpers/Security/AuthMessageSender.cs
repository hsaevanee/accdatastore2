using ACCDataStore.Entity;
using ACCDataStore.Entity.AuthenIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ACCDataStore.Web.Helpers.Security
{
    public class AuthMessageSender
    {
        public AuthMessageSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public EmailSettings _emailSettings { get; set; }

        public Task SendEmailAsync(string email, string subject, string message, List<string> listResultFile)
        {

            Execute(email, subject, message, listResultFile).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message, List<string> listResultFile)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Speech2Text Alarm")
                };

                var arrEmail = toEmail.Split(',');
                if (arrEmail != null && arrEmail.Length > 0)
                {
                    foreach (var sEmail in arrEmail)
                    {
                        mail.To.Add(new MailAddress(sEmail.Trim()));
                    }
                }
                else
                {
                    mail.To.Add(new MailAddress(toEmail));
                }

                // mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (listResultFile != null && listResultFile.Count > 0)
                {
                    foreach (var sResultFile in listResultFile)
                    {
                        mail.Attachments.Add(new Attachment(sResultFile));
                    }
                }

                using (var smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}