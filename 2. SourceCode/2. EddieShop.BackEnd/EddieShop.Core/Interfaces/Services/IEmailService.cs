using EddieShop.Core.Entities;
using EddieShop.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Interfaces.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// created by ntdung5 21.01.2022
        ServiceResult SendEmail(Email email);
    }
}
