using MtSmart.BLL.DTO.AccountDTOs;
using System.Security.Claims;

namespace MtSmart.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IBaseResponse<ClaimsIdentity>> Login(UserLoginDTO model);
        Task<bool> RemindPassword(string email);
    }
}
