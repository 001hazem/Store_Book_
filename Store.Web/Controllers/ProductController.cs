using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Core.Dtos.CategoryDto;
using Store.Core.Dtos.ProductDto;
using Store.Infostructure;
using Store.Infrastructure.Servicess.CategoriesServicess;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Data;
using Store.Web.Models;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServicess _productServices;
        private readonly ICategoryServices _categoryServices;


        public ProductController(IProductServicess productServices, ICategoryServices categoryServices, IUserServices userServices) : base(userServices)
        {
            _productServices=productServices;
            _categoryServices=categoryServices;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string searchKey, int page)
        {
            var result = await _productServices.GetDataIndex(searchKey, page);
            ViewBag.searchKey = searchKey;

            return View(result);
        }

      

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productServices.Create(dto);
                TempData["success"] = "Product Created successfully";

                return RedirectToAction("Index");
            }
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            var user = await _productServices.Get(Id);
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");

            return View(user);

        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productServices.Update(dto);

                TempData["Message"] = "s: The Product Is updated successfully";
                return RedirectToAction("Index");

            }
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");

            return View(dto);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            await _productServices.Delete(Id);
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> updatedStatus(int Id, ContentStatus status)
        //{
        //    await _postServices.UpdateStatus(Id, status);
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public async Task<IActionResult> ChangeLogStatus(int Id)
        //{
        //    var log = await _postServices.ListChangeLog(Id);
        //    return View(log);
        //}
    }
}