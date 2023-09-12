using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Models
{
    public class OrderDetail :BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }     
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int ProductId { get; set; }        
        public Product Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
