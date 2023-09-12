using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.CategoryDto;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.Infrastructure.Servicess.CategoriesServicess
{ 
   public class CategoryServiecs : ICategoryServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryServiecs( ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;         
        }    
        
        public async Task<PaginationViewModel> GetDataIndex(string searchKey, int page)
        {
            var pageSize = 10.0;
            var numberOfPage = Math.Ceiling(await _context.Categories.CountAsync(x => !x.IsDelete && (x.Name.Contains(searchKey) || string.IsNullOrWhiteSpace(searchKey))) / pageSize);

            if (page <= 1 || page > numberOfPage)
            {
                page = 1;
            }

            var skipValue = (int)((page - 1) * pageSize);

            var GetAll = await _context.Categories.Where(x => !x.IsDelete && (x.Name.Contains(searchKey) || string.IsNullOrWhiteSpace(searchKey)))
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip(skipValue).Take((int)pageSize).ToListAsync();

            var Categorys = _mapper.Map<List<CategoryViewModel>>(GetAll);

            var paginationViewModel = new PaginationViewModel();
            paginationViewModel.NumberOfPage = (int)numberOfPage;
            paginationViewModel.CurrentPage = page;
            paginationViewModel.data = Categorys;

            return paginationViewModel;

        }
        public async Task<List<CategoryViewModel>> GetData()
        {
            var GetAll = await _context.Categories.Where(x => !x.IsDelete).ToListAsync();
            var categories = _mapper.Map<List<CategoryViewModel>>(GetAll);
            return categories;

        }
        public async Task<int> Create(CreateCategoryDto Input)
        {
            var CreateCategory = _mapper.Map<Category>(Input);
            await _context.Categories.AddAsync(CreateCategory);
            await _context.SaveChangesAsync();          
            return CreateCategory.Id;

        }
        public async Task<UpdateCategoryDto> Get(int id)
        {
            var GetItem = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete); if (GetItem == null)
            {
                //throw new IsTheUserNotExsit();
            }

            return _mapper.Map<UpdateCategoryDto>(GetItem);

        }
        public async Task<int> Update(UpdateCategoryDto dto)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == dto.Id);
            if (category == null)
            {
                //throw new EntityNotFoundException();
            }
            var updatedCategory = _mapper.Map<UpdateCategoryDto, Category>(dto, category);
            updatedCategory.UpdatedAt= DateTime.Now;
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
            return updatedCategory.Id;
        }
        public async Task<int> Delete(int id)
        {

            var delete = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (delete == null)
            {
                //throw new IsTheUserNotExsit();
            }

             delete.IsDelete = true;
            _context.Categories.Update(delete);
            await _context.SaveChangesAsync();
            return delete.Id;

        }


    }
}
