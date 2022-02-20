using EddieShop.Core.Entities;
using EddieShop.Core.Interfaces.Services;
using EddieShop.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EddieShop.Controller.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        IUploadService _uploadService;
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        /// <summary>
        /// Tải ảnh lên server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// created by ntdung 26.01.2022
        [HttpGet("image")]
        public IActionResult UploadImage()
        {
            try
            {
                var serviceResult = _uploadService.UploadImage();
                //4.Trả về kết quả cho client
                if (serviceResult.Success == true)
                {
                    return StatusCode(200, serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new ServiceResult();

                errorObj.Success = false;
                errorObj.Msg = ResourceVN.Exception_ErrorMsg;
                errorObj.DevMsg = ex.Message;
                errorObj.Code = "Eddie-001";
                errorObj.MoreInfo = "https://openapi.Eddie.com.vn/errorcode/Eddie-001";
                errorObj.TraceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb";

                return StatusCode(500, errorObj);
            }
        }
    }
}
