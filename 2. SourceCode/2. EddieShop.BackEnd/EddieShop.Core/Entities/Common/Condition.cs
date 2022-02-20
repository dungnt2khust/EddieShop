using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Entities.Common
{
    public class Condition
    {
        /// <summary>
        /// Tên trường
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Giá trị so sánh
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Kiểu so sánh
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public string Type { get; set; }
    }
}
