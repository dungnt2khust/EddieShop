﻿using EddieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Interfaces.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Kiểm tra tài khoản đúng
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (15/11/2021)
        /// ModifiedBy: NTDUNG (22/11/2021)
        ServiceResult checkValidAccount(Account account);

        /// <summary>
        /// Kiểm tra sessionID
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (15/11/2021)
        ServiceResult checkSession(Guid? sessionID);

        /// <summary>
        /// Đăng kí tài khoản mới
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (30/11/2021)
        /// ModifiedBy: NTDUNG (06/12/2021)
        ServiceResult registerAccount(User user);

        /// <summary>
        /// Chỉnh sửa thông tin tài khoản
        /// </summary>
        /// <param name="newInfo"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (08/12/2021)
        ServiceResult changeAccountInfo(object newInfo);
    }
}
