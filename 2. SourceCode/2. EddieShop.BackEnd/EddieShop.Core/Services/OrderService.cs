using EddieShop.Core.Entities;
using EddieShop.Core.Hubs;
using EddieShop.Core.Hubs.Interfaces;
using EddieShop.Core.Interfaces.Base;
using EddieShop.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EddieShop.Core.Services
{
    public class OrderService : BaseService<_Order>, IOrderService
    {
        IBaseRepository<_Order> _orderRepository;
        IBaseRepository<Cart> _cartRepository;
        IBaseRepository<Product> _productRepository;
        IBaseRepository<Admin> _adminRepository;
        IBaseRepository<User> _userRepository;
        ISignalRHub _signalRHub;
        public OrderService(IBaseRepository<_Order> baseRepository, IBaseRepository<Cart> cartRepository, IBaseRepository<Product> productRepository, IBaseRepository<Admin> adminRepository, IBaseRepository<User> userRepository, ISignalRHub signalRHub) : base(baseRepository)
        {
            _orderRepository = baseRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _signalRHub = signalRHub;
        }

        #region Insert
        /// <summary>
        /// Thêm mới đơn hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// CreatedBy: NTDUNG (06/01/2021)
        public override ServiceResult Insert(_Order entity, Guid? sessionID)
        {
            // Lấy mã đơn hàng mới
            entity.OrderCode = _orderRepository.GetNewCode(sessionID);

            // Gán ID
            entity.GetType().GetProperty($"_OrderID").SetValue(entity, Guid.NewGuid());

            // Thêm mới vào bảng đơn hàng
            var serviceResult = base.Insert(entity, sessionID);
            if (serviceResult.Success)
            {
                // Đổi trạng thái giỏ hàng thành đang trong đơn hàng
                var carts = entity.Carts;
                var rowEffectCarts = 0;
                var rowEffectProducts = 0;
                foreach(var cart in carts)
                {
                    var newCart = new Cart();
                    newCart.OrderID = entity._OrderID;
                    newCart.Active = 1;
                    var listColumns = new List<String>();
                    listColumns.Add("OrderID");
                    listColumns.Add("Active");
                    var rowEffectCart = _cartRepository.UpdateColumns(newCart, cart.CartID, listColumns, sessionID);
                    rowEffectCarts += rowEffectCart;

                    // Cộng vào số đã bán
                    var product = _productRepository.GetEntityById(cart.ProductID, sessionID, false);
                    product.Sold += cart.CartQuantity;
                    var rowEffectProduct = _productRepository.Update(product, product.ProductID, sessionID);
                    rowEffectProducts += rowEffectProduct;
                }

                if (rowEffectCarts != carts.Count() || rowEffectProducts != carts.Count())
                {
                    serviceResult.Success = false;
                    serviceResult.Msg = Resources.ResourceVN.Exception_ErrorMsg;
                }
            }
            if (serviceResult.Success)
            {

                var admins = _adminRepository.GetAllEntities(sessionID);
                var adminIDs = new List<Guid>();
                foreach(var admin in admins)
                {
                    adminIDs.Add((Guid)admin.AdminID);
                }
                var userSent = _userRepository.GetEntityById(entity.UserID, sessionID, false);
                _signalRHub.SendNotifyToAdmins(userSent, adminIDs, new { message = entity.OrderCode });
            }
            return serviceResult;
        }
        #endregion
    }
}
