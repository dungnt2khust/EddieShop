using EddieShop.Core.Entities;
using EddieShop.Core.Interfaces.Base;
using EddieShop.Core.Interfaces.Services;
using EddieShop.Core.Resources;
using EddieShop.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EddieShop.Controller.API.Controllers
{
    public class OrderController : BaseController<_Order>
    {
        IOrderService _orderService;
        public OrderController(IBaseService<_Order> baseService, IOrderService orderService) : base(baseService)
        {
            _orderService = orderService;
        }

        public override IActionResult Insert(_Order entity, Guid? sessionID)
        {
            try
            {
                //Trả về kết quả cho client
                var serviceResult = _orderService.Insert(entity, sessionID);
                if (serviceResult.Success)
                {
                    return StatusCode(201, serviceResult);
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
