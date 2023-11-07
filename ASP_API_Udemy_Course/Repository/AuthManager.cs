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
        public ApiUser _user { get; set; }

        public AuthManager(IMapper mapper, UserManager<ApiUser> roleManager, IConfiguration config)
        {

            this._mapper = mapper;
            this._userManager = roleManager;
            this._config = config;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            _user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (_user == null)
            {
                return null;
            }
            var Passwordcheck = await _userManager.CheckPasswordAsync(_user, loginDTO.PasswordHash);
            if (!Passwordcheck)
            {
                return null;
            }
            var token = await GenerateToken();
            return new AuthResponseDTO
            {
                ID = _user.Id,
                Username = _user.UserName,
                Token = token,
                RefreshToken = await GenerateRefreshToken()
            };
        }

        public async Task<string> GenerateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, "HotelListing_API", "RefreshToken");
            var refreshedtoken = await _userManager.GenerateUserTokenAsync(_user, "HotelListing_API", "RefreshToken");
            var result = await _userManager.SetAuthenticationTokenAsync(_user, "HotelListing_API", "RefreshToken", refreshedtoken);
            return refreshedtoken;
        }

        public async Task<AuthResponseDTO> VerifyRefreshToken(AuthResponseDTO request)
        {
            var JWTHandeler = new JwtSecurityTokenHandler();
            var tokencontent = JWTHandeler.ReadJwtToken(request.Token);
            var username = tokencontent.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            _user = await _userManager.FindByNameAsync(username);
            if (_user == null|| request.ID != _user.Id)
            {
                return null;
            }
            var result = await _userManager.VerifyUserTokenAsync(_user, "HotelListing_API", "RefreshToken", request.RefreshToken);
            if (result)
            {
                var token = await GenerateToken();
                return new AuthResponseDTO
                {
                    ID = _user.Id,
                    Username = _user.UserName,
                    Token = token,
                    RefreshToken = await GenerateRefreshToken()
                };
            }
            else
            {
               
                await _userManager.UpdateSecurityStampAsync(_user);
                return null;

            }


        }

        public async Task<IEnumerable<IdentityError>> Register(APIUser_DTO userDTO)
        {
            _user = _mapper.Map<ApiUser>(userDTO);
            _user.UserName = userDTO.Email;

            var result = await _userManager.CreateAsync(_user, userDTO.PasswordHash);

            //adding the user to the rolles 
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "USER1");
            }

            return result.Errors;


        }

        //making it internal cuz it wont be used outside the class
        internal async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWtAuthentication:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Roles = await _userManager.GetRolesAsync(_user);
            var RolesClaims = Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var Userclaims = await _userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, _user.Email),
                new Claim ("Id", _user.Id),
             }.Union(RolesClaims).Union(Userclaims);
            var token = new JwtSecurityToken(issuer: _config["JWtAuthentication:Issuer"],
                                              audience: _config["JWtAuthentication:Audience"],
                                              claims: claims,
                                              expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["JWtAuthentication:TokenExpirationInMinutes"])),
                                              signingCredentials: credentials
                                              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}



