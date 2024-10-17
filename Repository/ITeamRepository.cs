using Euroleague.DTO;
using Euroleague.Models;

namespace Euroleague.Repository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllTeams();
        Task<Team> GetTeamById(int Id);
        Task<Team> CreateTeam(TeamManipulationDTO teamManDTO);
        Task<TeamManipulationDTO> EditTeam(int id,TeamManipulationDTO article);
        Task DeleteTeam(int id);

        Task<string> SaveImage(IFormFile image);

        void DeleteImage(string imagePath);

    }
}
