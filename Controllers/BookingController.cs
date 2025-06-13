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
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO != null)
            {
                try
                {
                    TimeSpan difference = bookingDTO.DateEnd - bookingDTO.DateStart;
                    int days = difference.Days;

                    User user = this.ReturnCurrentUser(HttpContext);
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

                    string json = JSON.JsonOptions<Booking>(booking);
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

        [HttpPut("CancelCooking/{IDBooking}")]
        public async Task<IActionResult> CancelBooking([FromRoute] int IDBooking)
        {
            try
            {

                User user = this.ReturnCurrentUser(HttpContext);

                Booking bookingResult = await _context.Bookings
                    .Include(b => b.Car)
                    .FirstOrDefaultAsync(b => b.IDBooking == IDBooking);

                BookingController.ValidateBooking(bookingResult);

                //Only cancel if is not started
                if (bookingResult.BookingStatus != EBookingStatus.Reserved)
                {
                    throw new Exception("Can´t cancel the booking");
                }

                if (bookingResult.IDUser != user!.IDUser)
                {
                    throw new Exception("Only the owner of the booking can cancel his own booking");
                }

                bookingResult.BookingStatus = EBookingStatus.Cancelled;
                bookingResult.Car.CarStatus = ECarStatus.Available;

                _logger.Log(LogLevel.Information, $"{IDBooking} was cancelled");
            
                _context.SaveChanges();

                return Ok(bookingResult.BookingStatus);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("DeliverTheCar/{IDBooking}")]
        [Authorize(Roles = "worker")]
        public async Task<IActionResult> DeliverTheCar([FromRoute] int IDBooking)
        {
            Booking bookingResult = await _context.Bookings
                .Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.IDBooking == IDBooking);

            try
            {
                ValidateBooking(bookingResult);

                bookingResult.BookingStatus = EBookingStatus.InUse;

                return Ok("The client can acces to his car and drive it");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ReturnCar/{carPatent}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> ReturnCar([FromRoute] string carPatent)
        {

            DateTime timeCarReturned = DateTime.Now;

            string msg = "The booking has be completed";

            if (carPatent != null)
            {
                var query = _context.Bookings.AsQueryable();

                query = query.Where(b => b.CarPatent == carPatent)
                    .Where(b => b.BookingStatus == EBookingStatus.InUse)
                    .Include(b => b.Car);

                Booking booking = await query.FirstOrDefaultAsync();

                if (booking != null)
                {
                    booking.BookingStatus = EBookingStatus.Completed;
                    booking.Car.CarStatus = ECarStatus.Available;


                    //If the customer deliver the car after the dateEnd, has a extra cost
                    TimeSpan difference = timeCarReturned - booking.DateEnd;

                    if(difference.Days  > 0)
                    {
                        int extra = difference.Days * 10;
                        booking.Cost += extra;

                        _logger.Log(LogLevel.Information, $"{booking.IDBooking} id booking already completed");
                        _context.SaveChanges();
                        return Ok(msg + $", and you have to pay an extra that is $ {extra} USD");
                    }
                    else
                        return Ok(msg);

                }
                else
                    return NotFound("Invalid Car patent");
            }
            else
                return BadRequest("Invalid Data");
        }

        [HttpGet("MyBookings")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> ReturnAllUserBookings()
        {
            User user = this.ReturnCurrentUser(HttpContext);

            List<Booking> bookings = await _context.Bookings
                                    .AsNoTracking() //If i only want to show the Booking data without other data like User and Car (in this case), use this!
                                    .Where(b => b.IDUser == user.IDUser)
                                    .ToListAsync();

            string json = JSON.JsonOptions<List<Booking>>(bookings);

            return Ok(json);
        }

        [HttpGet("AllBookings")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            List<Booking> bookings = _context.Bookings.ToList();

            string json = JSON.JsonOptions<List<Booking>>(bookings);

            return Ok(json);

        }

        public static void ValidateBooking(Booking bookingResult)
        {
            if (bookingResult == null)
            {
                throw new Exception("Incorrect ID");
            }

        }

        public User ReturnCurrentUser(HttpContext htpp)
        {
            //We dont have to retrieve the JWT manually, HttpContext.User is responsible for do this
            var userId = htpp.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _context.Users.Find(int.Parse(userId));
        }

    }

}
