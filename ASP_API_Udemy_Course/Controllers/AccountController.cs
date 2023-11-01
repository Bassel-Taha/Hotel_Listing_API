using ASP_API_Udemy_Course.Contract;
using ASP_API_Udemy_Course.Models.DTO_refoactored_classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace ASP_API_Udemy_Course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            this._authManager = authManager;
        }

        //Post: api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Register( [FromBody] APIUser_DTO aPIUser_DTO)
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
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> login (LoginDTO loginDTO)
        {
            var Loginvalidation =  await _authManager.Login(loginDTO);
            if (!Loginvalidation)
            {
                return BadRequest("Invalid Login Attempt");
            }
            
            return Ok("valid login attempt");
        }
    }
}
