using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Core.Dtos.CategoryDto;
using Store.Core.Dtos.ProductDto;
using Store.Core.Dtos.UserDto;
using Store.Infostructure;
using Store.Infrastructure.Servicess.CategoriesServicess;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Data;
using Store.Web.Models;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly ICategoryServices _categoryServices;


        public UserController( ICategoryServices categoryServices, IUserServices userServices) : base(userServices)
        {
            _categoryServices=categoryServices;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string searchKey, int page)
        {
            var result = await _userServices.GetDataIndex(searchKey, page);
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
        public async Task<IActionResult> Create([FromForm] CreateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userServices.Create(dto);
                TempData["success"] = "User Created successfully";
                return RedirectToAction("Index");
            }
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await _userServices.Get(Id);

            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userServices.Update(dto);

                TempData["Message"] = "s: The User Is updated successfully";
                return RedirectToAction("Index");

            }
            ViewData["categoryData"] = new SelectList(await _categoryServices.GetData(), "Id", "Name");

            return View(dto);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            await _userServices.Delete(Id);
            TempData["success"] = "User deleted successfully";
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