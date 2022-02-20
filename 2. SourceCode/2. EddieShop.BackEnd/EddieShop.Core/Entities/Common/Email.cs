using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Entities.Common
{
    public class Email : BaseEntity
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gửi từ
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Nhắc đến
        /// </summary>
        public List<string> CC { get; set; }

        /// <summary>
        /// Gửi đến
        /// </summary>
        public List<string> To { get; set; }
    }
}
