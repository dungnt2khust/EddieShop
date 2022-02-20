using EddieShop.Core.Entities;
using EddieShop.Core.Interfaces.Repository;
using EddieShop.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Services
{
    public class UploadService : IUploadService
    {
        IUploadRepository _uploadRepository;
        public UploadService(IUploadRepository uploadRepository)
        {
            _uploadRepository = uploadRepository;
        }
        /// <summary>
        /// Tải ảnh lên server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// created by ntdung5 26.01.2022
        public ServiceResult UploadImage()
        {
            try
            {
                var serviceResult = new ServiceResult();
                serviceResult.Data = _uploadRepository.UploadImage();
                return serviceResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
