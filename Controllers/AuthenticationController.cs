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
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IConfiguration config, FastDriveContext context, ILogger<AuthenticationController> logger)
        {
            _config = config;
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO loginDto)
        {
            if (loginDto != null)
            {
                try
                {
                    var query =  _context.Users.AsQueryable();
                    query = query.Where( u => u.Email == loginDto.Email);
                    query = query.Where( u => u.Password == loginDto.Password);

                    User user = query.FirstOrDefault();

                    if (user == null)
                    {
                        throw new Exception("User doesn´t exists");
                    }
                    return Ok(JWT.GenerateJwtToken(user));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
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
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    _logger.LogInformation("New user registered");
                    return Ok(JWT.GenerateJwtToken(user));
                }
                else
                    return BadRequest();

            }
            else
                return BadRequest("Invalid Data");
        }

    }
}
