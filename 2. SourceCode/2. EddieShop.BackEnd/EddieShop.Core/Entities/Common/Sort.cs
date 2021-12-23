using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Entities.Common
{
    public class Sort
    {
        /// <summary>
        /// Tên trường dữ liệu
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Giảm hay tăng
        /// </summary>
        public bool Desc { get; set; }
    }
}
