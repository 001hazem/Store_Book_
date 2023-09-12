using Store.Core.Dtos.OrderHeader;
using Store.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.CartServicess
{
    public interface  ICartServices
    {
        void ClearCart(string userId);

		Task<List<ShopingCartViewModel>> GetData(string userId);
        //Task<int> Delete(int id);
        int Plus(int cartId);
        int Minus(int cartId);
        int Remove(int cartId);
        Task<int> CreateOne(string userId, List<ShopingCartViewModel> shoppingCartItems);  
        Task<int> OrderConfirmation(int Id);
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
		void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
	}
}
