using Master.Core.DTO;
using Master.Core.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Core.Interfaces
{
    public interface IAuthService
    {
        BaseResponse<UserDto> GetUserDetails();
        Task<BaseResponse<string>> Login(HttpContext httpContext);
        Task<BaseResponse<string>> LogOut(HttpContext httpContext);
    }
}
