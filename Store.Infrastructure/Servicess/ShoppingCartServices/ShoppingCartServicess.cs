using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.ProductDto;
using Store.Core.Exceptions;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infostructure;
using Store.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.ShoppingCartServicess
{
    public class ShoppingCartServicess : IShoppingCartServicess
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ShoppingCartServicess( ApplicationDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

       
        public async Task<ShoppingCartDetailsViewModel> Detail(int productId)
        {
            var GetAll = await _context.ShoppingCarts.Include(x => x.Product).Include(x=>x.User).SingleOrDefaultAsync(x => x.Id == productId );

            if (GetAll == null)
            {
                throw new EntityNotFoundException();

            }
            var products = _mapper.Map<ShoppingCartDetailsViewModel>(GetAll);
            return products;
        }

        

    }
}

