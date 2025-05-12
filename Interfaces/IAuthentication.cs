using FastDrive.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastDrive.Interfaces
{
    public interface IAuthentication
    {
        public IActionResult Login([FromBody]string username, [FromBody] string password);

        public IActionResult Register([FromBody] User user);

        public string GenerateJwtToken(User user);
    }
}
