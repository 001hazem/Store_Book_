using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess;
using Store.Web.Data;
using System.Security.Claims;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Infrastructure.Servicess.CartServicess;
using Microsoft.AspNetCore.Authorization;
using Store.Core.Dtos.OrderHeader;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;
using Stripe;
using Store.Infrastructure.Servicess.OrderHeaderServicess;

namespace Store.Web.Controllers
{
    public class OrderHeaderController : Controller
    {
        private readonly ILogger<PublicController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserServices _UserServices;
        private readonly IOrderHeaderServiecs _OrderHeaderServices;


        public OrderHeaderController(ILogger<PublicController> logger, IOrderHeaderServiecs OrderHeaderServices,
            IMapper mapper, ApplicationDbContext context, IUserServices UserServices)
        {
            _UserServices = UserServices;
            _context = context;
            _mapper = mapper;
			_OrderHeaderServices = OrderHeaderServices;
            _logger = logger;
        }

      
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var shoppingCartItems = await _OrderHeaderServices.GetData();
            return Json(new { data= shoppingCartItems });
       
        }

    }
}

