using EddieShop.Core.Entities;
using EddieShop.Core.Entities.Common;
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
    public class EmailController : ControllerBase
    {
        #region Declare
        IEmailService _EmailService;
        #endregion

        #region Constructor
        public EmailController(IEmailService EmailService)
        {
            _EmailService = EmailService;
        }
        #endregion

        #region Methods
        #region SendEmail
        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// created by ntdung5 21.01.2022
        [HttpPost]
        public IActionResult SendEmail(Email email)
        {
            try
            {
                var serviceResult = _EmailService.SendEmail(email);
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
        #endregion
        #endregion
    }
}
