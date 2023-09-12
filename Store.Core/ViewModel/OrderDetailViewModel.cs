using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.ViewModel
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        public OrderHeaderViewModel OrderHeader { get; set; }
        [Required]
        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
