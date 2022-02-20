using Imagekit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Interfaces.Repository
{
    public interface IUploadRepository
    {
        /// <summary>
        /// Tải ảnh lên server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// created by ntdung5 26.01.2022
        AuthParamResponse UploadImage();
    }
}
