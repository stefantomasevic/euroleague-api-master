using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Euroleague.Data;
using Euroleague.Models;
using Euroleague.Repository;
using Euroleague.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace Euroleague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    public class TeamController : ControllerBase
    {
        ///
        private readonly ApplicationDbContext _context;

        private readonly ITeamRepository _repository;

        private readonly IMapper _mapper;

        public TeamController(ApplicationDbContext context, ITeamRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Team
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            var teams = await _repository.GetAllTeams();


            if (teams == null || !teams.Any())
            {
                return NotFound();
            }
            var teamsDTO = _mapper.Map<IEnumerable<TeamDTO>>(teams);

            return Ok(teamsDTO);
        }

        // GET: api/Team/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var team = await _repository.GetTeamById(id);

            if (team == null)
            {
                return NotFound();
            }
            var teamsDTO = _mapper.Map<TeamDTO>(team);
            return teamsDTO;
        }

        // PUT: api/Team/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id,[FromForm]TeamManipulationDTO teamManDTO)
        {
            try { 
            var editedTeam = await _repository.EditTeam(id, teamManDTO);

            if (editedTeam == null)
            {
                return NotFound(); // Tim nije pronađen
            }

            return Ok(editedTeam);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST: api/Team/post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost("postTeam")]
     
        public async Task<ActionResult<Team>> PostTeam([FromForm] TeamManipulationDTO teamManDTO)
        {
            try
            {
                var createdTeam = await _repository.CreateTeam(teamManDTO);

                return createdTeam;
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }


        }

        // DELETE: api/Team/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try {
                await _repository.DeleteTeam(id);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        private bool TeamExists(int id)
        {
            return (_context.Teams?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        
    }
}
