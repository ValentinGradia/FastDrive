using FastDrive.Models;
using FastDrive.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FastDrive.Interfaces
{
    public interface IAuthentication
    {
        public Task<IActionResult> Login([FromBody] UserDTO userDto);

        public Task<IActionResult> Register([FromBody] User userDto);
    }
}
