using FastDrive.Data;
using FastDrive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class BookingController : Controller
    {

        private readonly FastDriveContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(FastDriveContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

    }
}
