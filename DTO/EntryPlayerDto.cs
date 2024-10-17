using Euroleague.Models;

namespace Euroleague.DTO
{
    public class EntryPlayerDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Position { get; set; }

        public string Nationality { get; set; }

        public int TeamId { get; set; }

        public IFormFile logo { get; set; }

    }
}
