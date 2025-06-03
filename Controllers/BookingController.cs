using AutoMapper;
using FastDrive.Data;
using FastDrive.Models;
using FastDrive.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class BookingController : Controller
    {

        private readonly FastDriveContext _context;
        private readonly ILogger<BookingController> _logger;
        private readonly IMapper _mapper;

        public BookingController(FastDriveContext context, ILogger<BookingController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        
        [HttpPost("NewBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO != null)
            {
                try
                {
                    //We dont have to retrieve the JWT manually, HttpContext.User is responsible for do this
                    var currentUser = HttpContext.User;
                    var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    User user = await _context.Users.FindAsync(int.Parse(userId));
                    Car car = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == bookingDTO.PatentCar);

                    if (car != null)
                    {
                        Booking booking = _mapper.Map<Booking>(bookingDTO);
                        booking.IDUser = user!.IDUser;
                        booking.User = user;
                        booking.Car = car!;

                        _context.Bookings.Add(booking);
                        _context.SaveChanges();
                        return Ok("Booking already saved");
                    }

                    throw new Exception("Car doesn´t exists");
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
