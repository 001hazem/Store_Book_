using Microsoft.AspNetCore.Mvc;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Models;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
   

        public HomeController(ILogger<HomeController> logger, IUserServices userServices) : base(userServices)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
      
            return View();
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