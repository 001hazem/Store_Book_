using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Store.Core.ViewModel
{
    public class ShoppingCartDetailsViewModel
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public ProductDetailsViewModel Product { get; set; }
        public UserViewModel users { get; set; }
        public int Count { get; set; }
        public string UserId { get; set; }
    }
}
