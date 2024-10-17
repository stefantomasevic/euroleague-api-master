using AutoMapper;
using Euroleague.Data;
using Euroleague.DTO;
using Euroleague.Models;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;

namespace Euroleague.Repository
{
    public class SqlTeamRepository : ITeamRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SqlTeamRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Team> CreateTeam(TeamManipulationDTO teamManDTO)
        {
            if (_context.Teams.Any(t => t.Name == teamManDTO.Name))
            {
                throw new InvalidOperationException("Team alredy exist");
            }

            var team = _mapper.Map<Team>(teamManDTO);

            if (teamManDTO.Logo != null)
            {
                team.LogoPath = await SaveImage(teamManDTO.Logo);
            }

            _context.Teams.Add(team);

            await _context.SaveChangesAsync();

            return team;
        }

        public async Task DeleteTeam(int id)
        {

           
            


            var deletedTeam = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);

            if (deletedTeam != null)
            {
                _context.Teams.Remove(deletedTeam);
                await _context.SaveChangesAsync(); // Sačuvaj promene
            }

        }

        public async  Task<TeamManipulationDTO> EditTeam(int teamId,TeamManipulationDTO teamManDTO)
        {
            
                var existingTeam = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == teamId);
                

                if (existingTeam == null)
                {
                    // Tim nije pronađen
                    return null;
                }

                // Sačuvaj trenutnu putanju slike pre nego što je promenite
                var currentImagePath = existingTeam.LogoPath;

            // Mapiranje izmena sa DTO objekta na postojeći tim
                _mapper.Map(teamManDTO, existingTeam);
                existingTeam.Id = teamId;

                // Ako je promenjena slika, obrišite trenutnu sliku i sačuvajte novu
                if (teamManDTO.Logo != null)
                {
                    if (!string.IsNullOrEmpty(currentImagePath))
                    {
                        DeleteImage(currentImagePath);
                    }

                    existingTeam.LogoPath = await SaveImage(teamManDTO.Logo);
                }

                await _context.SaveChangesAsync();
            _mapper.Map(existingTeam, teamManDTO);

                return teamManDTO;
            
        }

        public async  Task<IEnumerable<Team>> GetAllTeams()
        {
            return await _context.Teams.Include(t => t.Players).ToListAsync();
 
        }

        public async Task<Team> GetTeamById(int Id)
        {
            var team = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == Id);

            if (team == null)
            {
                return null;
            }

            return team;
        }
        

        public async Task<string> SaveImage(IFormFile image)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

            // path for picture
            var filePath = Path.Combine("wwwroot", "images", "logos", uniqueFileName);

            // save picture on server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // return path
            return Path.Combine("images", "logos", uniqueFileName);
        }
        public void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine("wwwroot", imagePath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

       
    }
}
