    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Euroleague.Models
    {
        public class Schedule
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public int HomeId { get; set; }
            public Team HomeTeam { get; set; }
            public int GuestId { get; set; }
            public Team GuestTeam { get; set; }
            public DateTime GameTime { get; set; }
            public int? HomeScore { get; set; } 
            public int? GuestScore { get; set; }
        }
    }
