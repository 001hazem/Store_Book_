using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ViewModel
{
    public class ShopingCartViewModel
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public ProductViewModel Product { get; set; }
        public UserViewModel User { get; set; }
        public int Count { get; set; }
        public string UserId { get; set; }
        public double orderTotal { get; set; }
    }
}
