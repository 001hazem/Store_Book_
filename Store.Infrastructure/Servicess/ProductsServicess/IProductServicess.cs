using Store.Core.Dtos.ProductDto;
using Store.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.ProductsServicess
{
    public interface IProductServicess
    {
        Task<List<ProductViewModel>> GetData();
        Task<PaginationViewModel> GetDataIndex(string searchKey, int page);
        Task<int> Update(UpdateProductDto dto);
        Task<int> Create(CreateProductDto Input);
        Task<UpdateProductDto> Get(int id);
        Task<ProductDetailsViewModel> Detail(int productId);
        Task<int> Delete(int id);
    }
}
