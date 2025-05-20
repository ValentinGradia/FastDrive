using AutoMapper;
using FastDrive.Data;
using FastDrive.Interfaces;
using FastDrive.Models;
using FastDrive.Models.AutoMapperModels;
using FastDrive.Models.DTO;
using FastDrive.Models.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.Annotations;
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
        private readonly FastDriveContext _context;

        public AuthenticationController(IConfiguration config, FastDriveContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO loginDto)
        {
            if (loginDto != null)
            {
                try
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Password == loginDto.Password);
                    return Ok(GenerateJwtToken(user));
                }
                catch (Exception ex)
                {
                    return BadRequest("User doesn´t exists");
                }
            }
            return BadRequest("Invalid Data");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user != null)
            {
                UserValidator validator = new UserValidator();
                ValidationResult result = validator.Validate(user);


                if (result.IsValid)
                {
                    await _context.Users.AddAsync(user);
                    _context.SaveChanges();
                    return Ok(GenerateJwtToken(user));
                }
                else
                    return BadRequest();

            }
            else
                return BadRequest("Invalid Data");
        }

        private string GenerateJwtToken(User user)
        {
            // Create a symmetric security key using a secret key from the configuration.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Specify the signing credentials using the security key and HMAC-SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims for the JWT token, including username and role.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name), // Claim representing the user's name.
                new Claim(ClaimTypes.Role, user.UserType.ToString())   // Claim representing the user's role.
            };

            // Create a JWT token with specified claims, expiration, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: null, // No specific issuer specified.
                audience: null, // No specific audience specified.
                claims: claims, // Pass the claims defined above.
                expires: DateTime.Now.AddHours(1), // Set token expiration to 1 hour from now.
                signingCredentials: credentials); // Include signing credentials for the token.
            // Serialize the JWT token into a string and return it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
