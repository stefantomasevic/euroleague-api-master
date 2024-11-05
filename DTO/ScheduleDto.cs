using Euroleague.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euroleague.DTO
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int HomeId { get; set; }
        public string HomeTeam { get; set; }
        public int GuestId { get; set; }
        public string GuestTeam { get; set; }
        public DateTime GameTime { get; set; }
        public int? HomeScore { get; set; }
        public int? GuestScore { get; set; }
        public string HomeLogo { get; set; }
        public string GuestLogo { get; set; }
    }
}
