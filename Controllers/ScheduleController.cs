using AutoMapper;
using Euroleague.Data;
using Euroleague.DTO;
using Euroleague.Models;
using Euroleague.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Euroleague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IScheduleRepository _repository;

        private readonly IMapper _mapper;

        public ScheduleController(ApplicationDbContext context, IScheduleRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("createSchedule")]

        public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] CreateScheduleDto createScheduleDto)
        {
            try
            {
                var createdSchedule = await _repository.CreateSchedule(createScheduleDto);

                return createdSchedule;
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

        }
        [HttpGet("{date}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedule(DateTime date)
        {
            try
            {
                var schedule = await _repository.GetScheduleByDate(date);



                return Ok(schedule);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
