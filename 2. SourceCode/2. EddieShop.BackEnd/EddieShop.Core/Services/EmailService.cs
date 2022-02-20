using EddieShop.Core.Entities;
using EddieShop.Core.Entities.Common;
using EddieShop.Core.Interfaces.Repository;
using EddieShop.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EddieShop.Core.Services
{
    public class EmailService : IEmailService
    {
        #region Declare
        IEmailRepository _emailRepository;
        #endregion

        #region Contructor
        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }
        #endregion
        public ServiceResult SendEmail(Email email)
        {
            try
            {
                var serviceResult = new ServiceResult();

                // Validate email
                if (email.To.Count() == 0)
                {
                    serviceResult.Msg = "Bạn phải điền người nhận";
                    serviceResult.Success = false;
                    return serviceResult;
                }
                var checkMailSend = IsValidEmail(email.From);
                if (!checkMailSend)
                {
                    serviceResult.Msg = "Email người gửi không hợp lệ";
                    serviceResult.Success = false;
                    return serviceResult;
                }

                if (email.To.Count() > 0)
                {

                    foreach (var mail in email.To)
                    {
                        var checkMailReceive = IsValidEmail(mail);
                        if (!checkMailReceive)
                        {
                            serviceResult.Msg = "Email người nhận không hợp lệ";
                            serviceResult.Success = false;
                            return serviceResult;
                        }
                    }
                }

                if (email.CC.Count() > 0)
                {
                    foreach (var mail in email.CC)
                    {
                        var checkMailCC = IsValidEmail(mail);
                        if (!checkMailCC)
                        {
                            serviceResult.Msg = "Email CC không hợp lệ";
                            serviceResult.Success = false;
                            return serviceResult;
                        }
                    }

                }


                serviceResult.Data = _emailRepository.SendEmail(email);
                return serviceResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
