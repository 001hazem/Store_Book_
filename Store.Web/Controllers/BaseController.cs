using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Infrastructure.Servicess.UserServicess;

namespace Store.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly IUserServices _userServices;


        public BaseController(IUserServices userServices)
        {
            _userServices = userServices;
           
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _userServices.GetUserNameAndData(userName);

                ViewBag.UserName = user.Email;
                ViewBag.email = user.Email;
                ViewBag.image = user.ImageUrl;
                ViewBag.userType = user.UserType;

            }
        }
    }
}
