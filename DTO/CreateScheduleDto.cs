namespace Euroleague.DTO
{
    public class CreateScheduleDto
    {
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public DateTime GameTime { get; set; }
    }
}
