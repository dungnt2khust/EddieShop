using System;
using System.Collections.Generic;
using System.Text;
using static EddieShop.Core.Attributes.EddieShopAttributes;

namespace EddieShop.Core.Entities
{
    public class Cart : BaseEntity
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        public Guid CartID { get; set; }

        /// <summary>
        /// Người dùng
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int CartQuantity { get; set; }

        /// <summary>
        /// Trạng thái 
        /// 0 - Hết hàng, 1 - Vửa xinh, 2 - Hàng khả dụng có thể chọn thêm, 3 - Không đủ hàng
        /// </summary>
        [EddieNotMap]
        public int? Status { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        [EddieNotMap]
        public decimal CartTotalPrice { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [EddieNotMap]
        public string ProductName { get; set; }

        /// <summary>
        /// Mã Sản phẩm
        /// </summary>
        [EddieNotMap]
        public string ProductCode { get; set; }

        /// <summary>
        /// Giá bán
        /// </summary>
        [EddieNotMap]
        public decimal Price { get; set; }

        /// <summary>
        /// Hình ảnh
        /// </summary>
        [EddieNotMap]
        public string Image { get; set; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        [EddieNotMap]
        public int Quantity { get; set; }

        /// <summary>
        /// Id đơn hàng
        /// </summary>
        [EddieNotMap]
        public Guid? OrderID { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Active { get; set; }
    }
}
