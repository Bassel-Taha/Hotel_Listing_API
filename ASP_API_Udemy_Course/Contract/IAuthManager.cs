using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes.UsersDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_API_Udemy_Course.Contract
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register (APIUser_DTO userDTO);

        Task<AuthResponseDTO> Login(LoginDTO loginDTO);

        Task<String> GenerateRefreshToken();

        Task<AuthResponseDTO> VerifyRefreshToken(AuthResponseDTO request);

    }
}
