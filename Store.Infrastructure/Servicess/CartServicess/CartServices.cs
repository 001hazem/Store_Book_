using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.OrderHeader;
using Store.Core.Exceptions;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infostructure;
using Store.Web.Data;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;

namespace Store.Infrastructure.Servicess.CartServicess
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;


        public CartServices(ApplicationDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;

        }

		public async Task<int> CreateOne(string userId, List<ShopingCartViewModel> shoppingCartItems)
		{
			// Calculate the total order value using LINQ
			double totalOrderValue = shoppingCartItems.Sum(item => item.Product.Price * item.Count);

			// Check if the cart is not empty
			if (shoppingCartItems.Count == 0)
			{
				// If the cart is empty, you can handle this case, like showing a message to the user
				throw new EntityNotFoundException();
			}

			// Assuming you have a model for the order
			var order = new OrderHeader
			{
				userId = userId,
				OrderDate = DateTime.Now,
				OrderTotal = totalOrderValue
			};

			var user = shoppingCartItems.FirstOrDefault()?.User; // Assuming the User property exists in the ShoppingCartViewModel

			if (user != null)
			{
				order.Name = user.Name;
				order.City = user.City;
				order.PostalCode = user.PostalCode;
				order.PhoneNumber = user.PhoneNumber;
				order.State = user.State;
				order.StreetAddress = user.StreetAddress;

				await _context.OrderHeader.AddAsync(order);
				await _context.SaveChangesAsync();

				// Assuming you have a model for order items
				foreach (var item in shoppingCartItems)
				{
					var orderItem = new OrderDetail
					{
						OrderHeaderId = order.Id,
						ProductId = item.Product.Id,
						Count = item.Count,
						Price = item.Product.Price
					};

					await _context.OrderDetail.AddAsync(orderItem);
				}

				await _context.SaveChangesAsync();

				// Clear the shopping cart after the order is placed
				ClearCart(userId);

				// You can now redirect the user to the order confirmation page or return the order ID
				return order.Id;
			}

            // Return a default or error value here if user is null (optional)
            return -1; // For example, you can return -1 if there's an issue with the user data
        }
		
     
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
		{
			var orderFromDb = _context.OrderHeader.FirstOrDefault(u => u.Id == id);
			if (!string.IsNullOrEmpty(sessionId))
			{
				orderFromDb.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				orderFromDb.PaymentIntentId = paymentIntentId;
				orderFromDb.PaymentDate = DateTime.Now;
			}

		}


		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _context.OrderHeader.FirstOrDefault(u => u.Id == id);
			if (orderFromDb != null)
			{
				orderFromDb.OrderStatus = orderStatus;
			
                if (!string.IsNullOrEmpty(paymentStatus))
				{
					orderFromDb.PaymentStatus = paymentStatus;
				}
			}
		}


		public void ClearCart(string userId)
		{
			// Get the shopping cart items associated with the user
			var cartItems = _context.ShoppingCarts.Where(item => item.UserId == userId);

			// Remove the cart items from the database
			_context.ShoppingCarts.RemoveRange(cartItems);

			// Save the changes to the database
			_context.SaveChanges();
		}
		public async Task<List<ShopingCartViewModel>> GetData(string userId)
        {
            var shoppingCartList = await _context.ShoppingCarts
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(u => u.UserId == userId)
                .ToListAsync();

            var listCartShoping = _mapper.Map<List<ShopingCartViewModel>>(shoppingCartList);

            // Calculate the order total for each ShopingCartViewModel using the GetPriceBasedQuantity method
            foreach (var cartViewModel in listCartShoping)
            {
                double price = GetPriceBasedQuantity(shoppingCartList, cartViewModel);
                cartViewModel.orderTotal += price * cartViewModel.Count;
            }

            return listCartShoping;
        }
        private double GetPriceBasedQuantity(List<ShoppingCart> shoppingCartList, ShopingCartViewModel shopingCartViewModel)
        {
            // Find the corresponding ShoppingCart object from the shoppingCartList
            ShoppingCart shoppingCart = shoppingCartList.Find(cart => cart.Id == shopingCartViewModel.Id);

            // Perform the price calculation based on the shoppingCart object
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }

        }
		public async Task< int> OrderConfirmation(int Id)
		{
			var orderHeader =await _context.OrderHeader.Include(x => x.user).SingleOrDefaultAsync(u => u.Id == Id);
			//_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book",
			//	$"<p>New Order Created - {orderHeader.Id}</p>");

			List<ShoppingCart> shoppingCarts = _context.ShoppingCarts.Where(u => u.UserId == orderHeader.userId).ToList();

			_context.ShoppingCarts.RemoveRange(shoppingCarts);
			_context.SaveChanges();
            return orderHeader.Id;

		}
		public int Plus(int cartId)
        {
            var cartFromDb = _context.ShoppingCarts.SingleOrDefault(u => u.Id == cartId);
            if (cartFromDb == null)
            {
                throw new EntityNotFoundException();
            }
            cartFromDb.Count += 1;
            _context.ShoppingCarts.Update(cartFromDb);
            _context.SaveChanges();
            return cartFromDb.Id;
        }
        public int Minus(int cartId)
        {
            var cartFromDb = _context.ShoppingCarts.SingleOrDefault(u => u.Id == cartId);
            if (cartFromDb == null)
            {
                throw new EntityNotFoundException();
            }

            if (cartFromDb.Count <= 1)
            {
                //remove that from cart

                _context.ShoppingCarts.Remove(cartFromDb);
                
            }
            else
            {
                cartFromDb.Count -= 1;
                _context.ShoppingCarts.Update(cartFromDb);
            }

            _context.SaveChanges();
            return cartFromDb.Id;
        }
        public int Remove(int cartId)
        {
            var cartFromDb = _context.ShoppingCarts.SingleOrDefault(u => u.Id == cartId);

            if (cartFromDb == null)
            {
                throw new EntityNotFoundException();
            }
            _context.ShoppingCarts.Remove(cartFromDb);
            _context.SaveChanges();
            return cartFromDb.Id;
        }


    }
}
