namespace Euroleague.DTO
{
    public class TeamManipulationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Coach { get; set; }
        public string Arena { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
