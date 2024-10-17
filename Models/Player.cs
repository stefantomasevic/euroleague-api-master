using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Euroleague.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Position { get; set; }

        public string Nationality { get; set; }

        public string ImagePath { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }




    }
}
