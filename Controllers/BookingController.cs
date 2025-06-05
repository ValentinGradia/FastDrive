using AutoMapper;
using FastDrive.Data;
using FastDrive.Models;
using FastDrive.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                    TimeSpan difference = bookingDTO.DateEnd - bookingDTO.DateStart;
                    int days = difference.Days;

                    User user = await _context.Users.FindAsync(int.Parse(userId));
                    Car car = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == bookingDTO.PatentCar);

                    if (car != null )
                    {
                        if (user != null)
                        {

                            if (car.CarStatus == ECarStatus.Available)
                            {

                                Booking booking = _mapper.Map<Booking>(bookingDTO);
                                car.CarStatus = ECarStatus.Booked; //EF already update this within the Car table of the DB

                                if(user.Bookings == null)
                                {
                                    user.Bookings = new List<Booking>();
                                }
                                user.Bookings.Add(booking);
                                booking.IDUser = user!.IDUser;
                                booking.User = user;
                                booking.Car = car!;
                                booking.Cost = days * 10;

                                _context.Bookings.Add(booking);
                                _logger.Log(LogLevel.Information, "New booking");
                                _context.SaveChanges();
                                return Ok($"Booking already saved, the IDBooking is: {booking.IDBooking} and will cost you USD{days * 10}$");
                            }
                            else
                                return BadRequest("The car is not available");
                        }
                        else
                            throw new Exception("User doesn´t exists");
                    }
                    else
                        throw new Exception("Car doesn´t exists");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                
            }

            return BadRequest("Invalid data");


        }

        [HttpGet("GetBoooking/{IDBooking}")]
        public async Task<IActionResult> GetBooking([FromRoute] int IDBooking)
        {
            try
            {
                Booking booking = await _context.Bookings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.IDBooking == IDBooking);

                if (booking != null)
                {

                    //Solving the error of " A possible object cycle was detected which is not supported." when i give this into a json
                    JsonSerializerOptions options = new()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        WriteIndented = true,
                        MaxDepth = 0,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    string json = JsonSerializer.Serialize(booking, options);
                    return Ok(json);
                }
                else
                    throw new Exception("Incorect ID");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
