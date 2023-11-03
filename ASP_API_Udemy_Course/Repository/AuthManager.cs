using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes.UsersDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP_API_Udemy_Course.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _config;

        public AuthManager(IMapper mapper, UserManager<ApiUser> roleManager, IConfiguration config)
        {

            this._mapper = mapper;
            this._userManager = roleManager;
            this._config = config;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
            {
                return null;
            }
            var Passwordcheck = await _userManager.CheckPasswordAsync(User, loginDTO.PasswordHash);
            if (!Passwordcheck)
            {
                return null;
            }
            var token = await GenerateToken(User);
            return new AuthResponseDTO
            {
                ID = User.Id,
                Username = User.UserName,
                Token = token

            };

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

        public async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWtAuthentication:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Roles = await _userManager.GetRolesAsync(user);
            var RolesClaims = Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var Userclaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub, user.Email),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, user.Email),
                new Claim ("Id", user.Id),
             }.Union(RolesClaims).Union(Userclaims);
            var token = new JwtSecurityToken (issuer: _config["JWtAuthentication:Issuer"],
                                              audience: _config["JWtAuthentication:Audience"],
                                              claims: claims,
                                              expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["JWtAuthentication:TokenExpirationInMinutes"])),
                                              signingCredentials: credentials
                                              );
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }

            
    }

}

