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
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> Logger)
        {
            this._authManager = authManager;
            this._logger = Logger;
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

            _logger.LogInformation($"regestrign a new user account ");
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, $"something went wring with {nameof(Register)}");
                return Problem($"something wnt wrong in {nameof(Register)}", statusCode: 500);
            }
            
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
            _logger.LogInformation($"attempt of logging with the username : {loginDTO.Email}");
            try
            {
                var user_token = await _authManager.Login(loginDTO);
                if (user_token == null)
                {
                    return BadRequest("Invalid Login Attempt");
                }
                return Ok(user_token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex , $"login error with {nameof(login)} using username : {loginDTO.Email}");
                return Problem($"something went wrong in the login", statusCode : 500 );
            }
            
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
            _logger.LogInformation($"generating and congirming refresh token with a new token for username : {request.Username}");
            var user_refresh_token = await _authManager.VerifyRefreshToken(request);
            if (user_refresh_token == null)
            {
                _logger.LogError($"something went wrong in creating a refresh token using {nameof(VerifyRefreshToken)}");
                return BadRequest("Invalid Login Attempt");
            }
            return Ok(user_refresh_token);
        }
    }
}
