using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_API_Udemy_Course.Contract
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register (APIUser_DTO userDTO);
        Task<bool> Login(LoginDTO loginDTO);
    }
}
