namespace Euroleague.Models
{
    public class Statistic
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int PlayerId { get; set; }
        public int Rebounds { get; set; }
        public int Points { get; set; }
        public int Asists { get; set; }
        public int Fouls { get; set; }

        public Player Player { get; set; }
        public Schedule Schedule { get; set; }
    }
}
