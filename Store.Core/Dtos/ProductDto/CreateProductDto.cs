using Microsoft.AspNetCore.Http;
using Store.Core.Dtos.CategoryDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Store.Core.Dtos.ProductDto
{
    public class CreateProductDto
    {


        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Description")]

        public string Description { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }
        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "ProductImages")]
        public IFormFile ImagesUrl { get; set; }
    }
}
