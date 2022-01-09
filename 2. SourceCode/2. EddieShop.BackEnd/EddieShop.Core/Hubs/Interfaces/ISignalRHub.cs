using EddieShop.Core.Entities;
using EddieShop.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EddieShop.Core.Hubs.Interfaces
{
    public interface ISignalRHub
    {
        /// <summary>
        /// Cập nhật connectionID mới cho tài khoản
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (15/11/2021)
        /// ModifiedBy: NTDUNG (19/11/2021)
        ServiceResult UpdateConnectionID(AccountData accountData);

        /// Gửi tin nhắn đến user 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task SendMessageToSpecialUser(User userSent, string userName, string message);

        /// <summary>
        /// Gửi tin nhắn đến người dùng
        /// </summary>
        /// <param name="userSent"></param>
        /// <param name="usersReceive"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (24/11/2021)
        Task SendMessageToUsers(User userSent, List<Guid> userReceiveIDs, string message);

        /// <summary>
        /// Gửi tin nhắn đến người dùng
        /// </summary>
        /// <param name="userSent"></param>
        /// <param name="usersReceive"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (24/11/2021);
        Task SendNotifyToUsers(User userSent, List<Guid> userReceiveIDs, object notify);

        /// <summary>
        /// Gửi tin nhắn đến quản trị viên
        /// </summary>
        /// <param name="userSent"></param>
        /// <param name="usersReceive"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (24/11/2021);
        Task SendNotifyToAdmins(User userSent, List<Guid> adminReceiveIDs, object notify);
    }
}
