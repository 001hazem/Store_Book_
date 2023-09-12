using Store.Core.Dtos.CategoryDto;
using Store.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.CategoriesServicess
{
    public interface ICategoryServices
    {
        Task<PaginationViewModel> GetDataIndex(string searchKey, int page);
        Task<int> Create(CreateCategoryDto Input);
        Task<List<CategoryViewModel>> GetData();
        Task<UpdateCategoryDto> Get(int id);
        Task<int> Update(UpdateCategoryDto dto);
        Task<int> Delete(int id);

    }
}
