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

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

    }
}
