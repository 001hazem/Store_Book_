using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Core.Dtos.CategoryDto;
using Store.Infrastructure.Servicess.CategoriesServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Models;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ILogger<HomeController> logger, ICategoryServices categoryServices, IUserServices userServices ):base(userServices)
        {
            _categoryServices = categoryServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey, int page)
        {
            var result = await _categoryServices.GetDataIndex(searchKey, page);
            ViewBag.searchKey = searchKey;
            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto dto)
        {
            if (dto.Name == dto.Display.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder Cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                await _categoryServices.Create(dto);
                TempData["success"]= "Category Created successfully";
                return RedirectToAction("Index");
            }
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var user = await _categoryServices.Get(Id);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UpdateCategoryDto dto)
        {
            if (dto.Name == dto.Display.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder Cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                await _categoryServices.Update(dto);
                TempData["update"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            await _categoryServices.Delete(Id);
            TempData["error"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}