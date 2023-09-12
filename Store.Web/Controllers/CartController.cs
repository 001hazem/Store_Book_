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

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IShoppingCartServicess _shoppingCartServices;
        private readonly IProductServicess _productServices;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserServices _UserServices;
        private readonly ICartServices _CartServices;


        public CartController(ILogger<PublicController> logger, ICartServices CartServices, IMapper mapper, ApplicationDbContext context, IProductServicess productServices, IUserServices UserServices, IShoppingCartServicess shoppingCartServices)
        {
            _shoppingCartServices = shoppingCartServices;
            _UserServices = UserServices;
            _productServices = productServices;
            _context = context;
            _mapper = mapper;
            _CartServices = CartServices;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Get the shopping cart items from the _CartServices
            var shoppingCartItems = await _CartServices.GetData(userId);
            // Calculate the total order value using LINQ
            double totalOrderValue = shoppingCartItems.Sum(item => item.Product.Price * item.Count);
            // Pass the total order value to the view using ViewData
            ViewData["TotalOrderValue"] = totalOrderValue;
            return View(shoppingCartItems);
        }

        public IActionResult PLUS(int cartId)
        {
            _CartServices.Plus(cartId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            _CartServices.Minus(cartId);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get the shopping cart items from the _CartServices
            var shoppingCartItems = await _CartServices.GetData(userId);

            // Calculate the total order value using LINQ
            double totalOrderValue = shoppingCartItems.Sum(item => item.Product.Price * item.Count);

            foreach (var item in shoppingCartItems)
            {
                ViewData["Name"] = item.User.Name;
                ViewData["PostalCode"] = item.User.PostalCode;
                ViewData["PhoneNumber"] = item.User.PhoneNumber;
                ViewData["State"] = item.User.State;
                ViewData["City"] = item.User.City;
                ViewData["StreetAddress"] = item.User.StreetAddress;
            }

            // Pass the total order value to the view using ViewData

            ViewData["TotalOrderValue"] = totalOrderValue;

            return View(shoppingCartItems);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPOST()
        {
            // Get the user's identity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCartItems = await _CartServices.GetData(userId);

            // Calculate total order value
            double totalOrderValue = shoppingCartItems.Sum(item => item.Product.Price * item.Count);


            // Create the order in the database
            int orderId = await _CartServices.CreateOne(userId, shoppingCartItems);


            var domain = "https://localhost:7226/";
            // Configure the session options
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain +$"cart/OrderConfirmation?id={orderId}",
                CancelUrl = domain+"cart/index",
				LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
			};

            foreach (var item in shoppingCartItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                       
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },

                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }
            
			var service = new SessionService();

            Session session = service.Create(options);

            _CartServices.UpdateStripePaymentID(orderId, session.Id, session.PaymentIntentId);

            _CartServices.UpdateStatus(orderId, session.Status);
          
            _context.SaveChanges();

            Response.Headers.Add("Location", session.Url);

			return RedirectToAction(nameof(OrderConfirmation), new
			{
				id = orderId
			});

	}
        			

        public async Task< IActionResult> OrderConfirmation(int Id)
	    {
            var order =await _CartServices.OrderConfirmation(Id);

		    return View(order);
	    }



		public IActionResult Delete(int cartId)
        {
            _CartServices.Remove(cartId);

            return RedirectToAction(nameof(Index));
        }
        
    }
}

