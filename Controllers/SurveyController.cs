using AutoMapper;
using FastDrive.Data;
using FastDrive.Models;
using FastDrive.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Namotion.Reflection;
using static System.Net.WebRequestMethods;
using System.Security.Claims;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly FastDriveContext _context;
        private readonly ILogger<SurveyController> _logger;
        private readonly IMapper _mapper;

        public SurveyController(FastDriveContext context, ILogger<SurveyController> logger, IMapper mapper)
        {
            
            this._context = context;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpPost("NewSurvey")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> NewSurvey([FromBody] SurveyDTO surveyDTO)
        {
            if (surveyDTO != null )
            {
                Survey survey = _mapper.Map<Survey>(surveyDTO);

                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                survey.IDUser = int.Parse(userId);

                _context.Surveys.Add(survey);

                _context.SaveChanges();

                return Ok("Survey saved succesfully");
            }
            else
                return BadRequest("Invalid Data");
        }

        [HttpGet("BestSurveys")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetBestSurveys()
        {
            List<Survey> srveys = _context.Surveys.Where(s => s.ServiceCalification > 7).ToList();

            string json = JSON.JsonOptions<List<Survey>>(srveys);

            return Ok(json);
        }

        [HttpGet("WorstSurveys")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetWorstSurveys()
        {
            List<Survey> srveys = _context.Surveys.Where(s => s.ServiceCalification < 4).ToList();

            string json = JSON.JsonOptions<List<Survey>>(srveys);

            return Ok(json);
        }
    }
}
