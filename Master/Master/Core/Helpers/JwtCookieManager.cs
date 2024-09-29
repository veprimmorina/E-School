using Master.Core.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace Master.Core.Helpers
{
    public class JwtCookieManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtCookieManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetJwtToken(string token)
        {
            var cookieOptions = new CookieOptions
            {
                //HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);
        }

        public string GetJwtToken()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
        }

        public void RemoveJwtToken()
        {
            var cookie = GetJwtToken();

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "jwtToken",
                cookie,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Secure = true,
                    SameSite = SameSiteMode.None
                }
            );
        }

        public string GetEmailFromJwtToken()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == "email");

            return emailClaim?.Value;
        }

        public int GetIdFromJwtToken()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                return 0;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var idClaim = token.Claims.FirstOrDefault(c => c.Type == "Id");


            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            else
            {
                return 0;
            }
        }

        internal UserDto GetUserDetails()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(c => c.Type == "sub");
            var surname = token.Claims.FirstOrDefault(c => c.Type == "Surname");

            var response = new UserDto
            {
                FirstName = name.ToString().Split(":")[1],
                LastName = surname.ToString(),
            };

            return response;
        }
    }

}