using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolgozat0128.models
{
    public class Orders
    {
        public int order_id { get; set; }
        public int customer_id { get; set; }
        public string order_date { get; set; }
        public string status { get; set; }
        public string shipping_method { get; set; }
        public string payment_method { get; set; }
        public string notes { get; set; }
        public int total {  get; set; }
        public List<OrderItems> items { get; set; }

    }

    public class OrderItems
    {
        public int part_id { get; set; }
        public int quantity { get; set; }
        public int unit_price { get; set; }
        public int total {  get; set; }
    }
}
