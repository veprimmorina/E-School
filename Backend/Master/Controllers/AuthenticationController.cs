using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Master.Core.Helpers;
using Master.Core.Interfaces;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthService _authenticationService;
        private readonly JwtCookieManager _cookieManager;

        public AuthenticationController(IAuthService authenticationService, JwtCookieManager cookieManager)
        {
            _authenticationService = authenticationService;
            _cookieManager = cookieManager;

        }

        [HttpGet("signin")]
        public IActionResult GoogleLogin(string returnUrl = "/Authentication/handleRequest")
        {
            var isUserAlreadyLoggedIn = _cookieManager.GetJwtToken();

            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("You are logged in");
            }

            if (isUserAlreadyLoggedIn != null)
            {
                return BadRequest("You are logged in");
            }
            var properties = new AuthenticationProperties { RedirectUri = returnUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("handleRequest")]
        public async Task<IActionResult> GoogleCallback()
        {
            var response = await _authenticationService.Login(HttpContext);
            return Redirect(response.Message);
        }

        [HttpGet("returnLoginPage")]
        public async Task<IActionResult> ReturnLoginPage()
        {
            return Redirect("http://localhost:3000");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authenticationService.LogOut(HttpContext);
            return CreateResponse(response);
        }

        [HttpGet("getUserDetails")]
        public async Task<IActionResult> GetUserDetails()
        {
            var response = _authenticationService.GetUserDetails();
            return CreateResponse(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("This endpoint is accessible only by admins.");
        }
    }
}