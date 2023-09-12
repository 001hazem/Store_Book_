using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Store.Core.ViewModel
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public double ListPrice { get; set; }      
        public double Price { get; set; }
        public double Price50 { get; set; }
        public double Price100 { get; set; }
        public CategoryViewModel Category { get; set; }
        public string ImagesUrl { get; set; }
 

    }
}
