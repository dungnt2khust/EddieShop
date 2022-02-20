using EddieShop.Core.Interfaces.Repository;
using EddieShop.Core.Utilities;
using Imagekit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Infrastructure.Repository
{
    public class UploadRepository : IUploadRepository
    {
        /// <summary>
        /// Tải ảnh lên server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// created by ntdung5 26.01.2022
        [Obsolete]
        public AuthParamResponse UploadImage()
        {
            var uploadImage = new UploadImage();
            var result = uploadImage.Upload();
            return result;
        }
    }
}
