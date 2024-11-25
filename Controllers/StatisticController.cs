using AutoMapper;
using Euroleague.Data;
using Euroleague.Models;
using Euroleague.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Euroleague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        // GET: api/<StatisticController>
        private readonly ApplicationDbContext _context;

        private readonly IStatisticRepository _repository;

        private readonly IMapper _mapper;

        public StatisticController(ApplicationDbContext context, IStatisticRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("getStatisticByScheduleId/{scheduleId}")]
        public async Task<IActionResult> GetStatisticsByScheduleId(int scheduleId)
        {
            // Pozivamo metodu iz repozitorijuma koja vrati statistiku prema scheduleId
            var statistics = await _repository.GetStatisticsByScheduleIdAsync(scheduleId);

            // Ako nema statistike, vraćamo 404 Not Found
            if (statistics == null)
            {
                return NotFound("No statistics found for this schedule.");
            }

            // Vraćamo listu statistike kao odgovor
            return Ok(statistics);
        }

        // GET api/<StatisticController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StatisticController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StatisticController>/5
        [HttpPut("{id}")]
        public void UpdateStatisticByScheduleId(int scheduleId, [FromBody] string value)
        {
        }

        // DELETE api/<StatisticController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
