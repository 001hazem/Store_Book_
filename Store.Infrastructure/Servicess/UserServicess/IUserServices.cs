using Store.Core.Dtos.UserDto;
using Store.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.UserServicess
{
    public interface IUserServices
    {
        Task<PaginationViewModel> GetDataIndex(string searchKey, int page);
        UserViewModel GetUserNameAndData(string username);
        Task<List<UserViewModel>> GetListUser();
        Task<string> Create(CreateUserDto dto);
        Task<string> Update(UpdateUserDto dto);
        Task<string> Delete(string Id);
        Task<UpdateUserDto> Get(string Id);
        Task<string> Register(CreateUserDto dto);
    }
}
