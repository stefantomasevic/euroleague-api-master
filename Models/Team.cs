using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euroleague.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Coach { get; set; }

        public string Arena { get; set; }

        public string LogoPath { get; set; }

        public ICollection<Player> Players { get; set; }

  


    }
}
