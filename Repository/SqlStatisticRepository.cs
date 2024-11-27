    using AutoMapper;
    using Euroleague.Data;
    using Euroleague.DTO;
    using Euroleague.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using System.Drawing;
using Euroleague.Hubs;
using Microsoft.AspNetCore.SignalR;

    namespace Euroleague.Repository
    {
        public class SqlStatisticRepository : IStatisticRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
        private readonly IHubContext<StatisticHub> _hubContext;


        public SqlStatisticRepository(ApplicationDbContext context, IMapper mapper, IHubContext<StatisticHub> hubContext)
            {
                _context = context;
                _mapper = mapper;
            _hubContext = hubContext;
        }
      
            public async Task<ScheduleStatisticsDto> GetStatisticsByScheduleIdAsync(int scheduleId)
            {
                var statistics =
                    from statistic in _context.Statistic
                    join player in _context.Players on statistic.PlayerId equals player.Id
                    join team in _context.Teams on player.TeamId equals team.Id
                    join schedule in _context.Schedule on statistic.ScheduleId equals schedule.Id
                    join homeTeam in _context.Teams on schedule.HomeId equals homeTeam.Id
                    join guestTeam in _context.Teams on schedule.GuestId equals guestTeam.Id
                    where schedule.Id == scheduleId
                    group new { statistic, player } by new
                    {
                        schedule.Id,
                        schedule.HomeId,
                        schedule.GuestId,
                        schedule.HomeScore,
                        schedule.GuestScore,
                        HomeTeamName = homeTeam.Name,
                        GuestTeamName = guestTeam.Name
                    } into grouped
                    select new ScheduleStatisticsDto
                    {
                        ScheduleId = grouped.Key.Id,
                        HomeScore = grouped.Key.HomeScore,
                        GuestScore = grouped.Key.GuestScore,
                        HomeTeam = grouped.Key.HomeTeamName,
                        GuestTeam = grouped.Key.GuestTeamName,
                        HomePlayers = grouped
                            .Where(g => g.player.TeamId == grouped.Key.HomeId)
                            .Select(g => new PlayerStatisticsDto
                            {
                                PlayerId = g.player.Id,
                                PlayerName = g.player.FirstName + " " + g.player.LastName,
                                Statistics = g.statistic
                            })
                            .ToList(),
                        GuestPlayers = grouped
                            .Where(g => g.player.TeamId == grouped.Key.GuestId)
                            .Select(g => new PlayerStatisticsDto
                            {
                                PlayerId = g.player.Id,
                                PlayerName = g.player.FirstName + " " + g.player.LastName,
                                Statistics = g.statistic
                            })
                            .ToList()
                    };

                return  statistics.FirstOrDefault();
            }


            public async Task UpdateAssistsAsync(int scheduleId, int playerId, int assists)
            {

            var statistic =   await _context.Statistic
                                    .SingleOrDefaultAsync(s => s.ScheduleId == scheduleId && s.PlayerId == playerId);

            if (statistic != null)
                {
                    statistic.Asists += assists;
                     await  _context.SaveChangesAsync();

                var updatedStatistics = await GetStatisticsByScheduleIdAsync(scheduleId);

                // Slanje cele statistike klijentima preko SignalR-a
                await _hubContext.Clients.All.SendAsync("StatisticsUpdated", updatedStatistics);
            }
            }

            public async Task UpdateFoulsAsync(int scheduleId, int playerId, int fouls)
            {
                var statistic = await  _context.Statistic
                     .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId && s.PlayerId == playerId);

                if (statistic != null)
                {
                    statistic.Fouls += fouls;
                   await  _context.SaveChangesAsync();

                var updatedStatistics = await GetStatisticsByScheduleIdAsync(scheduleId);

                // Slanje cele statistike klijentima preko SignalR-a
                await _hubContext.Clients.All.SendAsync("StatisticsUpdated", updatedStatistics);
            }
            }

            public async Task UpdatePointsAsync(int scheduleId, int playerId, int points)
            {
                var statistic = await  _context.Statistic
                    .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId && s.PlayerId == playerId);

                if (statistic != null)
                {
                    statistic.Points += points;
                    await _context.SaveChangesAsync();

                var updatedStatistics = await GetStatisticsByScheduleIdAsync(scheduleId);

                // Slanje cele statistike klijentima preko SignalR-a
                await _hubContext.Clients.All.SendAsync("StatisticsUpdated", updatedStatistics);
            }
            }

            public async Task UpdateReboundsAsync(int scheduleId, int playerId, int rebounds)
            {
                var statistic =  await _context.Statistic
                     .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId && s.PlayerId == playerId);

                if (statistic != null)
                {
                    statistic.Rebounds += rebounds;
                     await _context.SaveChangesAsync();

                var updatedStatistics = await GetStatisticsByScheduleIdAsync(scheduleId);

                // Slanje cele statistike klijentima preko SignalR-a
                await _hubContext.Clients.All.SendAsync("StatisticsUpdated", updatedStatistics);
            }
            }


        }
    }
    public class ScheduleStatisticsDto
    {
        public int ScheduleId { get; set; }
        public int? HomeScore { get; set; }
        public int? GuestScore { get; set; }
        public string HomeTeam { get; set; }
        public string GuestTeam { get; set; }
        public List<PlayerStatisticsDto> HomePlayers { get; set; }
        public List<PlayerStatisticsDto> GuestPlayers { get; set; }
    }

    public class PlayerStatisticsDto
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public Statistic Statistics { get; set; } // Pretpostavljam da je `Statistic` već definisana klasa
    }

