using FastDrive.Data;
using FastDrive.Models;
using FastDrive.Models.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "worker")] // Only a worker can handle the cars
    public class CarController : Controller
    {
        private readonly FastDriveContext _context;
        private readonly ILogger<CarController> _logger;

        public CarController(FastDriveContext context, ILogger<CarController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetByPatent/{id}")]
        public async Task<IActionResult> GetCarByPattent([FromRoute] string patentId)
        {
            if (patentId != null)
            {
                Car car = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == patentId);

                if (car != null)
                {
                    return Ok(car);
                }
                else
                    return NotFound("Car doesn´t exists");
            }
            else
                return BadRequest("Invalid Data");
        }

        [HttpGet("SearchCar")]
        public async Task<IActionResult> SearchCar([FromQuery] string? model, [FromQuery] int? minKm, [FromQuery] int? maxKm, [FromQuery] string brand)
        {
            var query = _context.Cars.AsQueryable();

            if(!string.IsNullOrWhiteSpace(model))
                query = query.Where(c => c.Model == model);

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(c => c.Brand == brand);

            if (minKm != null && maxKm != null)
            {
                if (minKm > 0 && maxKm > 0)
                    query = query.Where(c => c.Km >= minKm && c.Km <= maxKm);
            }

            List<Car> cars = await query.ToListAsync();

            return Ok(cars);
        }

        [HttpGet("GetAllCars")]
        public async Task<IActionResult> GetAllCars()
        {
            return Ok(await _context.Cars.ToListAsync());
        }



        [HttpPost("AddCar")]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            if (car != null)
            {
                CarValidator validator = new CarValidator();
                ValidationResult result = validator.Validate(car);


                if (result.IsValid)
                {
                    if (!(_context.Cars.Contains(car)))
                    {

                        await _context.Cars.AddAsync(car);
                        _context.SaveChanges();
                        _logger.LogInformation("New car saved on the DB");
                        return Ok("Car already added");
                    }
                    else
                        return BadRequest("The car already exists");
                }
                else
                    return BadRequest();

            }
            else
                return BadRequest("Invalid Data");
        }
    }
}
