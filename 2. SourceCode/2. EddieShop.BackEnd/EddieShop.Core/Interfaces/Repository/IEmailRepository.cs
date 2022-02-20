using EddieShop.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Interfaces.Repository
{
    public interface IEmailRepository
    {
        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// created by ntdung 21.01.2022
        Object SendEmail(Email email);
    }
}
