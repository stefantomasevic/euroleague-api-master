namespace Euroleague.DTO
{
    public class TeamDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Coach { get; set; }

        public string Arena { get; set; }

        public string LogoPath { get; set; }

        public IEnumerable<PlayerDTO> Players { get; set; }
    }
}
