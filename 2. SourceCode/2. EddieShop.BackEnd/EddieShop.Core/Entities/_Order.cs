using System;
using System.Collections.Generic;
using System.Text;
using static EddieShop.Core.Attributes.EddieShopAttributes;

namespace EddieShop.Core.Entities
{
    public class _Order : BaseEntity
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid? _OrderID { get; set; }

        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Tham chiếu người dùng
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        [EddieNotMap]
        public int Quantity { get; set; }

        /// <summary>
        /// Tổng tiền
        /// </summary>
        [EddieNotMap]
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Id giỏ hàng 
        /// </summary>
        [EddieNotMap]
        public List<Cart> Carts { get; set; }
    }
}
