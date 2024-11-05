using Euroleague.DTO;
using Euroleague.Models;

namespace Euroleague.Repository
{
    public interface IScheduleRepository
    {
        Task<Schedule> CreateSchedule(CreateScheduleDto createScheduleDto);
        Task<IEnumerable<ScheduleDto>> GetScheduleByDate(DateTime date);
    }
}
