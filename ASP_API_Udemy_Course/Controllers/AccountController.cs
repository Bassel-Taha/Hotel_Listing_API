using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes.UsersDTO;
using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.IIS.Core;
using NuGet.Common;

namespace ASP_API_Udemy_Course.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            this._authManager = authManager;
        }

        //Post: api/account/register
        [HttpPost]
        [Route("register")]
        ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        // Register method to register a new user
        public async Task<IActionResult> Register([FromBody] APIUser_DTO aPIUser_DTO)
        {
            var errors = await _authManager.Register(aPIUser_DTO);
            if (errors.Any())
            {
                foreach (var er in errors)
                {
                    ModelState.AddModelError(er.Code, er.Description);
                }
                return BadRequest(ModelState);
            }


            return Ok();
        }

        //Post: api/account/login
        // the endpoint For the login must be Post
        [HttpPost]
        [Route("login")]
        ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        // Login method to authenticate user
        public async Task<IActionResult> login([FromBody] LoginDTO loginDTO)
        {

            var user_token = await _authManager.Login(loginDTO);
            if (user_token == null)
            {
                return BadRequest("Invalid Login Attempt");
            }     
            return Ok(user_token);
        }

        //Post: api/account/RefreshToken
        // the endpoint For refreshing the token must be Post
        [HttpPost]
        [Route("RefreshToken")]
        ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        // refresh token method to authenticate user
        public async Task<IActionResult> VerifyRefreshToken([FromBody] AuthResponseDTO request)
        {

            var user_refresh_token = await _authManager.VerifyRefreshToken(request);
            if (user_refresh_token == null)
            {
                return BadRequest("Invalid Login Attempt");
            }
            return Ok(user_refresh_token);
        }
    }
}
