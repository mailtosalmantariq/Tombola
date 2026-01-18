using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Beans.App.Service.Models
{
    public class OrderRequest
    {
        public string BeanExternalId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Email { get; set; } = string.Empty;
    }

}
