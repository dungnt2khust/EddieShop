using EddieShop.Core.Entities.Common;
using EddieShop.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EddieShop.Infrastructure.Repository
{
    public class EmailRepository : IEmailRepository
    {
        #region SendEmail
        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// created by ntdung5 21.01.2022
        public object SendEmail(Email email)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(email.From);
            mailMessage.Subject = email.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = @email.Body;

            // CC
            if (email.CC.Count() > 0)
            {
                foreach (var mail in email.CC)
                {
                    mailMessage.CC.Add(new MailAddress(mail));
                }
            }

            // To
            if (email.To.Count() > 0)
            {
                foreach (var mail in email.To)
                {
                    mailMessage.To.Add(new MailAddress(mail));
                }
            }

            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email.From, "Gmail@k63hust"),
                EnableSsl = true
            };
            // code in brackets above needed if authentication required

            try
            {
                client.Send(mailMessage);
                return new
                {
                    Message = "Success"
                };
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return new
                {
                    Message = "Fail"
                };

            }
        }

        #endregion
    }
}
