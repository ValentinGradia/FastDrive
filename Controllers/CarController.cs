using FastDrive.Data;
using FastDrive.Models;
using FastDrive.Models.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("GetByPatent/{patentId}")]
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
        //Params can be null
        public async Task<IActionResult> SearchCar([FromQuery] string? model, [FromQuery] string? brand, [FromQuery] int? carstatus)
        {
            var query = _context.Cars.AsQueryable();

            if(!string.IsNullOrWhiteSpace(model))
                query = query.Where(c => c.Model == model);

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(c => c.Brand == brand);

            if (carstatus != null && Enum.IsDefined(typeof(ECarStatus), carstatus))
            {
                query = query.Where(c => c.CarStatus == (ECarStatus)carstatus);
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

        [HttpPatch("ModifyCar/{patent}")]
        public async Task<IActionResult> PatchCar([FromRoute] string patent, [FromBody] JsonPatchDocument<Car> patch)
        {
            if(patch != null)
            {
                try
                {
                    Car car = await _context.Cars.FindAsync(patent);

                    patch.ApplyTo(car, ModelState);

                    _context.SaveChanges();

                    return Ok("Car modified succesfully");
                }
                catch (Exception ex)
                {
                    return BadRequest("Car doesn´t exists");
                }

            }
            return BadRequest("Invalid Data");
        }

        [HttpPut("UpdateCar/{patent}")]
        //Update all the attributes of a user
        public async Task<IActionResult> UpdateCar([FromBody] Car carParam)
        {
            if (carParam != null)
            {
                try
                {

                    Car car = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == carParam.Patent)!;

                    if (car == null)
                    {
                        throw new Exception("Car doesn´t exists");
                    }

                    _context.Entry(car).CurrentValues.SetValues(carParam);

                    await _context.SaveChangesAsync();

                    return Ok("Car updated succesfully");


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
