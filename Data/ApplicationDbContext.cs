using Euroleague.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Euroleague.Data
{
    public class ApplicationDbContext:DbContext
    {

       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
  


            modelBuilder.Entity<Team>()
            .HasMany(e => e.Players)
            .WithOne(e => e.Team)
            .HasForeignKey(e => e.TeamId)
            .IsRequired();

            modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                 .HasOne(s => s.HomeTeam)
                 .WithMany(t => t.HomeSchedules)
                 .HasForeignKey(s => s.HomeId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.GuestTeam)
                .WithMany(t => t.GuestSchedules)
                .HasForeignKey(s => s.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Statistic>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Statistics)
                .HasForeignKey(s => s.PlayerId);

            modelBuilder.Entity<Statistic>()
                .HasOne(s => s.Schedule)
                .WithMany(sc => sc.Statistics)
                .HasForeignKey(s => s.ScheduleId);

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Statistic> Statistic { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
