using AutoMapper;
using Store.Core.Dtos.CategoryDto;
using Store.Core.Dtos.ProductDto;
using Store.Core.Dtos.UserDto;
using Store.Core.ViewModel;
using Store.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.AutoMapper
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, UpdateCategoryDto>();

            CreateMap<Product, ProductViewModel>().ForMember(x => x.CreatedAt, x => x.MapFrom(x => x.CreatedAt.ToString("yyyy:MM:dd")));
            CreateMap<CreateProductDto, Product>().ForMember(x => x.ImagesUrl, x => x.Ignore());
            CreateMap<UpdateProductDto, Product>().ForMember(x => x.ImagesUrl, x => x.Ignore());
            CreateMap<Product, UpdateProductDto>().ForMember(x => x.ImagesUrl, x => x.Ignore());

            CreateMap<Product, ProductDetailsViewModel>();


            CreateMap<User, UserViewModel>().ForMember(x => x.UserType, x => x.MapFrom(x => x.UserType.ToString()));
            CreateMap<CreateUserDto, User>().ForMember(x => x.ImageUrl, x => x.Ignore());
            CreateMap<UpdateUserDto, User>().ForMember(x => x.ImageUrl, x => x.Ignore());
            CreateMap<User, UpdateUserDto>().ForMember(x => x.ImageUrl, x => x.Ignore());
            CreateMap<ShoppingCartDetailsViewModel, ShoppingCart>();

            CreateMap<ShoppingCart, ShopingCartViewModel>();
            // Map other properties as needed
            // If you want to support reverse mapping
            CreateMap<OrderHeader, OrderHeaderViewModel>();


        }
    }
}
