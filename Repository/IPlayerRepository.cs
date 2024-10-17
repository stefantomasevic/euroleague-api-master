using Euroleague.DTO;
using Euroleague.Models;

namespace Euroleague.Repository
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayers();
        Player GetPlayerById(int Id);
        Task<Player> CreatePlayer(EntryPlayerDto playerdTO);
        Player EditPlayer(Player player);
        void DeletePlayer(Player player);

        Task<IEnumerable<Player>> GetPlayersByTeamId(int teamId);

    }
}
