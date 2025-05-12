using FastDrive.Interfaces;
using FastDrive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller, IAuthentication
    {
        private readonly IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {   
                new Claim(ClaimTypes.Role, "Admin") 
            };

            var token = new JwtSecurityToken(
                issuer: null, // No specific issuer specified.
                audience: null, // No specific audience specified.
                claims: claims, // Pass the claims defined above.
                expires: DateTime.Now.AddHours(1), // Set token expiration to 1 hour from now.
                signingCredentials: credentials); // Include signing credentials for the token.

            // Serialize the JWT token into a string and return it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult Login([FromBody] string username, [FromBody] string password)
        {
            return Ok();
        }

        public IActionResult Register([FromBody] User user)
        {
            return Ok();
        }
    }
}
