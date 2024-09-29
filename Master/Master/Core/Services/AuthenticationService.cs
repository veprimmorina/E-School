using Master.Core.DTO;
using Master.Core.Helpers;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Master.Core.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly JwtCookieManager _jwtManager;
        private readonly IBaseIdentityRepository _baseIdentityRepository;

        public AuthenticationService(IConfiguration config, JwtCookieManager jwtManager, IBaseIdentityRepository baseIdentityRepository)
        {
            _config = config;
            _jwtManager = jwtManager;
            _baseIdentityRepository = baseIdentityRepository;
        }

        public async Task<BaseResponse<string>> LogOut(HttpContext httpContext)
        {
            try
            {
                await httpContext.SignOutAsync();

                var redirectUri = "https://localhost:7116/Authentication/signin";
                var googleLogoutUrl = $"https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue={redirectUri}";

                _jwtManager.RemoveJwtToken();
                return BaseResponse<string>.Success(redirectUri);
            }
            catch (Exception e)
            {
                return BaseResponse<string>.BadRequest($"An error occurred: {e.Message}");
            }
        }

        public async Task<BaseResponse<string>> Login(HttpContext httpContext)
        {

            var authenticateResult = await httpContext.AuthenticateAsync("Google");
            if (!authenticateResult.Succeeded)
            {
                return BaseResponse<string>.BadRequest("Unauthorized");
            }

            var googleUser = authenticateResult.Principal;

            var email = googleUser.FindFirst(ClaimTypes.Email)?.Value;
            var name = googleUser.FindFirst(ClaimTypes.Name)?.Value;
            var surname = googleUser.FindFirst(ClaimTypes.Surname)?.Value;

            var userExists = await _baseIdentityRepository.CheckIfUserExistsByEmail(email);

            if (!userExists)
            {
                return BaseResponse<string>.NotFound("User not found!");
            }

            var isUserConfirmed = await _baseIdentityRepository.CheckIfUserIsConfirmed(email);

            if (!isUserConfirmed)
            {
                return BaseResponse<string>.NotFound("User exists, but is not confirmed yet!");
            }

            var user = await _baseIdentityRepository.GetUserByEmail(email);
            name = user.FirstName + " " + user.LastName;
            surname = user.LastName;

            var userRole = await _baseIdentityRepository.GetUserRoles(email);
            List<string> roles;
            var token = "";
            roles = new List<string>();

            var redirect = "";


            if (userRole.Select(x => x.Role).Any(x => x.Contains("manager")))
            {
                roles.Add("manager");
                redirect = "http://localhost:3000/administrationStats";
            }
            else if (userRole.Select(x => x.Role).Any(x => x.Contains("teacher")))
            {
                roles.Add("editingteacher");
                redirect = "http://localhost:3000";
            }
            else if (userRole.Select(x => x.Role).Any(x => x.Contains("student")))
            {
                roles.Add("student");
                redirect = "http://localhost:3000/studentStats";
            }
            else
            {
                redirect = "http://localhost:3000/login";
            }

            token = GenerateJwtToken(name, surname, email, roles, userRole.FirstOrDefault().Id);
            _jwtManager.SetJwtToken(token);

            return BaseResponse<string>.Success(redirect);
        }

        private string GenerateJwtToken(string username, string surname, string email, IList<string> roles, int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("Surname", surname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("Id", id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public BaseResponse<UserDto> GetUserDetails()
        {
            var userDetails = _jwtManager.GetUserDetails();
            return BaseResponse<UserDto>.Success(userDetails);
        }
    }
}
