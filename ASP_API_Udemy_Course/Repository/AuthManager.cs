using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_API_Udemy_Course.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper , UserManager<ApiUser> roleManager)
        {
            
            this._mapper = mapper;
            this._userManager = roleManager;
        }
        public async Task<IEnumerable<IdentityError>> Register(APIUser_DTO userDTO)
        {
            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            
            var result = await _userManager.CreateAsync(user, userDTO.PasswordHash);
            
            //adding the user to the rolles 
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER1");
            }

            return result.Errors;

                
        }
    }
}
