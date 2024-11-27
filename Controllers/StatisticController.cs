using AutoMapper;
using Euroleague.Data;
using Euroleague.DTO.Entry;
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


        // PUT api/<StatisticController>/5
      
        [HttpPut("updateAsists/{scheduleId}")]
        public async Task UpdateAsistsByScheduleId(int scheduleId, [FromBody] AsistsDto asistDto)
        {
            await _repository.UpdateAssistsAsync(scheduleId, asistDto.PlayerId, asistDto.Asists);
        }

        [HttpPut("updatePoints/{scheduleId}")]
        public async Task UpdatePointsByScheduleId(int scheduleId, [FromBody] PointsDto pointsDto)
        {
            await _repository.UpdatePointsAsync(scheduleId, pointsDto.PlayerId, pointsDto.Points, pointsDto.IsHomePlayer);
        }

        [HttpPut("updateFouls/{scheduleId}")]
        public async Task UpdateFoulsByScheduleId(int scheduleId, [FromBody] FoulsDto foulsDto)
        {
            await _repository.UpdateFoulsAsync(scheduleId, foulsDto.PlayerId, foulsDto.Fouls);
        }

        [HttpPut("updateRebounds/{scheduleId}")]
        public async Task UpdateReboundsByScheduleId(int scheduleId, [FromBody] ReboundsDto reboundsDto)
        {
            await _repository.UpdateReboundsAsync(scheduleId, reboundsDto.PlayerId, reboundsDto.Rebounds);
        }

        [HttpPut("updateGame/{scheduleId}")]
        public async Task UpdateGameByScheduleId(int scheduleId, [FromBody] PointsDto reboundsDto)
        {
            await _repository.UpdateMatchAsync(scheduleId, reboundsDto.Points, reboundsDto.IsHomePlayer);
        }

    }
}
