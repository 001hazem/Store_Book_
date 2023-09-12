using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.UserDto;
using Store.Core.Enums;
using Store.Core.Exceptions;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infostructure;
using Store.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.UserServicess
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _da;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IFileService _file;

        public UserServices(RoleManager<IdentityRole> roleManager,  ApplicationDbContext da, UserManager<User> userManager, IMapper mapper, IFileService file)
        {
            _roleManager = roleManager;
            _da = da;
            _userManager = userManager;
            _mapper = mapper;
            _file = file;

        }

        public async Task<PaginationViewModel> GetDataIndex(string searchKey, int page)
        {
            var pageSize = 10.0;
            var numberOfPage = Math.Ceiling(await _da.Users.CountAsync(x => !x.IsDelete && (x.Name.Contains(searchKey) || string.IsNullOrWhiteSpace(searchKey))) / pageSize);

            if (page <= 1 || page > numberOfPage)
            {
                page = 1;
            }

            var skipValue = (int)((page - 1) * pageSize);

            var GetAll = await _da.Users.Where(x => !x.IsDelete && (x.Name.Contains(searchKey) || string.IsNullOrWhiteSpace(searchKey)))
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip(skipValue).Take((int)pageSize).ToListAsync();

            var Categorys = _mapper.Map<List<UserViewModel>>(GetAll);

            var paginationViewModel = new PaginationViewModel();
            paginationViewModel.NumberOfPage = (int)numberOfPage;
            paginationViewModel.CurrentPage = page;
            paginationViewModel.data = Categorys;

            return paginationViewModel;

        }
        public UserViewModel GetUserNameAndData(string username)
        {
            var user = _da.Users.SingleOrDefault(x => x.UserName == username && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(user);
        }              
        public async Task<List<UserViewModel>> GetListUser()
        {
            var GetAll = await _da.Users.Where(x => !x.IsDelete).ToListAsync();
            return _mapper.Map<List<UserViewModel>>(GetAll);
        }
        public async Task<string> Create(CreateUserDto dto)
        {
            await InitiRole();

            var Createuser = _da.Users.Any(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber && !x.IsDelete);
            if (Createuser)
            {
                //throw new TheUserIsExist();
            }
            var user = _mapper.Map<User>(dto);

            user.UserName = dto.Email;
            user.CreatedAt = DateTime.Now;

            user.UpdatedAt = DateTime.Now;

            if (dto.ImageUrl != null)
            {
                user.ImageUrl = await _file.SaveFile(dto.ImageUrl, "Images");

            }

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new OperationFailedException();
            }

           
             if (dto.UserType == UserType.Customer)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            else if (dto.UserType == UserType.Employee)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            else if (dto.UserType == UserType.Company)
            {
                await _userManager.AddToRoleAsync(user, "Company");
            }

            return user.Id;

        }
        public async Task<string> Update(UpdateUserDto dto)
        {
            var emailOrPhoneIsExist = _da.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber) && x.Id != dto.Id);
            if (emailOrPhoneIsExist)
            {
                throw new DuplicateEmailOrPhoneException();
            }


            var userId = await _da.Users.FindAsync(dto.Id);

            var updatedUser = _mapper.Map<UpdateUserDto, User>(dto, userId);
            updatedUser.UpdatedAt= DateTime.Now;
            if (dto.ImageUrl != null)
            {
                updatedUser.ImageUrl = await _file.SaveFile(dto.ImageUrl, "Images");
            }
            _da.Users.Update(updatedUser);
            await _da.SaveChangesAsync();
            return userId.Id;

        }
        public async Task<string> Delete(string Id)
        {
            var deleteUser = await _da.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);

            if (deleteUser == null)
            {
                throw new IsTheUserNotExsit();
            }

            deleteUser.IsDelete = true;
            _da.Users.Update(deleteUser);
            await _da.SaveChangesAsync();
            return deleteUser.Id;

        }

        public async Task<UpdateUserDto> Get(string Id)
        {
            var user = await _da.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateUserDto>(user);
        }

        public async Task InitiRole()
        {
            if (!_da.Roles.Any())
            {
                var roleManager = new List<string>();
                roleManager.Add("Admin");
                roleManager.Add("Customer");
                roleManager.Add("Employee");
                roleManager.Add("Company");

                foreach (var role in roleManager)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

            }

        }

        //private string GenratePassword()
        //{
        //    return Guid.NewGuid().ToString().Substring(1, 8);
        //}


        public async Task<string> Register(CreateUserDto dto)
        {            
            var Createuser = _da.Users.Any(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber && !x.IsDelete);
            if (Createuser)
            {
                //throw new TheUserIsExist();
            }
            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.UserType = UserType.Customer;

            if (dto.ImageUrl != null)
            {
                user.ImageUrl = await _file.SaveFile(dto.ImageUrl, "Images");
            }
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new OperationFailedException();
            }
            if (dto.UserType == UserType.Customer)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }           
            return user.Id;

        }



    }
}
