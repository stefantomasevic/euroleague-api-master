using Euroleague.DTO;
using Euroleague.Models;

namespace Euroleague.Repository
{
    public interface IStatisticRepository
    {
        Task UpdatePointsAsync(int scheduleId, int playerId, int points);

        Task UpdateReboundsAsync(int scheduleId, int playerId, int rebounds);

        Task UpdateAssistsAsync(int scheduleId, int playerId, int assists);

        Task UpdateFoulsAsync(int scheduleId, int playerId, int fouls);

        Task<ScheduleStatisticsDto> GetStatisticsByScheduleIdAsync(int scheduleId);
    }
}
