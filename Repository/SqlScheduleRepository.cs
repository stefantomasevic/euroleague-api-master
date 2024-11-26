using AutoMapper;
using Euroleague.Data;
using Euroleague.DTO;
using Euroleague.Models;
using Microsoft.EntityFrameworkCore;

namespace Euroleague.Repository
{
    public class SqlScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public SqlScheduleRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //public async Task<Schedule> CreateSchedule(CreateScheduleDto createScheduleDto)
        //{


        //    var schedule = new Schedule
        //    {
        //        HomeId = createScheduleDto.HomeTeamId,
        //        GuestId = createScheduleDto.GuestTeamId,
        //        GameTime = createScheduleDto.GameTime,
        //        HomeScore = 0,
        //        GuestScore = 0,
        //    };



        //    _context.Schedule.Add(schedule);

        //    await _context.SaveChangesAsync();

        //    return schedule;
        //}
        public async Task<Schedule> CreateSchedule(CreateScheduleDto createScheduleDto)
        {
            // Create the schedule
            var schedule = new Schedule
            {
                HomeId = createScheduleDto.HomeTeamId,
                GuestId = createScheduleDto.GuestTeamId,
                GameTime = createScheduleDto.GameTime,
                HomeScore = 0,
                GuestScore = 0,
            };

            _context.Schedule.Add(schedule);
            await _context.SaveChangesAsync();  // Save schedule first to get the schedule ID

            // Get the list of players for both teams (assuming these are already available)
            var homeTeamPlayers = await _context.Players.Where(p => p.TeamId == createScheduleDto.HomeTeamId).ToListAsync();
            var guestTeamPlayers = await _context.Players.Where(p => p.TeamId == createScheduleDto.GuestTeamId).ToListAsync();

            // Create statistics for home team players
            var homeTeamStatistics = homeTeamPlayers.Select(player => new Statistic
            {
                PlayerId = player.Id,
                ScheduleId = schedule.Id,
                Points = 0,
                Asists = 0,
                Rebounds = 0,
                Fouls = 0
            }).ToList();

            // Create statistics for guest team players
            var guestTeamStatistics = guestTeamPlayers.Select(player => new Statistic
            {
                PlayerId = player.Id,
                ScheduleId = schedule.Id,
                Points = 0,
                Asists = 0,
                Rebounds = 0,
                Fouls = 0
            }).ToList();

            // Add statistics to the context
            _context.Statistic.AddRange(homeTeamStatistics);
            _context.Statistic.AddRange(guestTeamStatistics);

            // Save changes to insert the statistics
            await _context.SaveChangesAsync();

            return schedule; // Return the created schedule
        }

        public async Task<IEnumerable<ScheduleDto>> GetScheduleByDate(DateTime date)
        {

            var startOfDay = date.Date; // Uzimamo samo datum bez vremena
            var endOfDay = startOfDay.AddDays(1);

            var schedules = await _context.Schedule
                    .Include(t => t.HomeTeam)
                    .Include(t => t.GuestTeam)
                    .Where(t => t.GameTime >= startOfDay && t.GameTime < endOfDay)
                    .Select(t => new ScheduleDto
                    {
                        Id = t.Id,
                        HomeId = t.HomeId,
                        HomeTeam = t.HomeTeam.Name,
                        GuestId = t.GuestId,
                        GuestTeam = t.GuestTeam.Name,
                        GameTime = t.GameTime,
                        HomeScore = t.HomeScore,
                        GuestScore = t.GuestScore,
                        HomeLogo = t.HomeTeam.LogoPath,
                        GuestLogo = t.GuestTeam.LogoPath
                    })
                    .ToListAsync();

            return schedules;
        }
    }
}
