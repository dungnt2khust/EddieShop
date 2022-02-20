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
        /// Trạng thái đơn hàng (0 - Đơn hàng mới tạo đang chờ duyệt, 1 - Đơn hàng đã duyệt, 2 - Đơn hàng đã đóng gói và vận chuyển, 3 - Đơn hàng đã được giao thành công)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Tổng tiền
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Id giỏ hàng 
        /// </summary>
        [EddieNotMap]
        public List<Cart> Carts { get; set; }
    }
}
