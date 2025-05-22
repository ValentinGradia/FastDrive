using Azure;
using FastDrive.Data;
using FastDrive.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly FastDriveContext _context;
        private readonly ILogger<UserController> _logger;


        public UserController(FastDriveContext context, ILogger<UserController> logger)
{
            _context = context;
            _logger = logger;
        }


        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByDNI([FromRoute] int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.IDUser == id);

            if (user != null)
                return Ok(user);
            else
                return NotFound("User doesn´t exists");
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _context.Users.ToListAsync();

            return Ok(users);
        }



        [HttpPatch("Modify/{id}")]
        [Authorize]
        //Only modify 1 or 2 attributes to the User
        public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            if(patchDoc != null)
            {
                try
                {

                    User user = await _context.Users.FindAsync(id);

                    patchDoc.ApplyTo(user, ModelState);

                    _context.SaveChanges();

                    return Ok("User modified succesfully");


                }
                catch (Exception ex)
                {
                    return BadRequest("User doesnt exists");
                }
            }

            return BadRequest("Invalid data");
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        //Update all the attributes of a user
        public async Task<IActionResult> UpdateUser([FromBody] User userParam)
        {
            if (userParam != null)
            {
                try
                {

                    User user = await _context.Users.FirstOrDefaultAsync(u => u.IDUser == userParam.IDUser)!;

                    if (user == null)
                    {
                        throw new Exception("User doesn´t exists");
                    }

                    _context.Entry(user).CurrentValues.SetValues(userParam);

                    await _context.SaveChangesAsync();

                    return Ok("User updated succesfully");


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return BadRequest("Invalid data");
        }



    }
}
