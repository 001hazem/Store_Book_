using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Store.Core.Dtos.CategoryDto
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Name Category")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Display")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int Display { get; set; }
    }
}
