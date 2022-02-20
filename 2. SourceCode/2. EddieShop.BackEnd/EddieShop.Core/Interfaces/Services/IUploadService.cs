using EddieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Interfaces.Services
{
    public interface IUploadService
    {
        /// <summary>
        /// Tải ảnh lên server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// created by ntdung5 26.01.2022
        ServiceResult UploadImage();
    }
}
