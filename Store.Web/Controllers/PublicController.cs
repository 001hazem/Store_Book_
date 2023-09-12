using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.UserDto;
using Store.Core.Exceptions;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infrastructure.Servicess;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Data;
using Store.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Store.Web.Controllers
{
    public class PublicController : Controller
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IShoppingCartServicess _shoppingCartServices;
        private readonly IProductServicess _productServices;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        private readonly IUserServices _UserServices;

        public PublicController(ILogger<PublicController> logger, IMapper mapper, ApplicationDbContext context, IProductServicess productServices,IUserServices UserServices, IShoppingCartServicess shoppingCartServices)
        {
            _shoppingCartServices = shoppingCartServices;
            _UserServices = UserServices;
            _productServices = productServices;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _productServices.GetData();
            return View(result);
        }

        
        public async Task<IActionResult> Details(int productId)
        {            
            var product = await _productServices.Detail(productId);               
            var shoppingCartDetailsViewModel = new ShoppingCartDetailsViewModel
            {
                Product = product,
                Count = 1,
                productId= productId

            };
            return View(shoppingCartDetailsViewModel);           
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details([FromForm]ShoppingCartDetailsViewModel shoppingCartDetailsViewModel)
        {
            var createShoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDetailsViewModel);

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            createShoppingCart.UserId = userId;

            var cartFromDb = _context.ShoppingCarts.SingleOrDefault(u => u.UserId == userId &&
               u.ProductId == shoppingCartDetailsViewModel.productId);
           
            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += createShoppingCart.Count;
                _context.ShoppingCarts.Update(cartFromDb);
                _context.SaveChanges();
                TempData["success"] = "Cart updated successfully";
            }
            else
            {
                //add cart record
                _context.ShoppingCarts.Add(createShoppingCart);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

      

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _UserServices.Register(dto);
                return RedirectToAction("Index");
            }

            return View(dto);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
