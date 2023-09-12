using Microsoft.AspNetCore.Http;
using Store.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Store.Core.Dtos.UserDto
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " EmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = "Date of Birth ")]
        public DateTime? DOB { get; set; }


        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " ImageUrl")]
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = "حجم الصورة يجب أن يكون 2 ميجابايت أو أقل.")]
        public IFormFile ImageUrl { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " UserType")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = " Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " StreetAddress")]
        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " City")]
        public string? City { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " State")]
        public string? State { get; set; }

        [Required(ErrorMessage = "The Field is requerd")]
        [Display(Name = " PostalCode")]
        public string? PostalCode { get; set; }
    }
}
