using Euroleague.Models;

namespace Euroleague.DTO
{
    public class StatisticForGameDto
    {
        public int ScheduleId { get; set; }
        public ICollection<Statistic> HomePlayers { get; set; }
        public ICollection<Statistic> GuestPlayers { get; set; }
    }
}
