﻿using AutoMapper;
using Euroleague.Data;
using Euroleague.DTO;
using Euroleague.Models;
using Microsoft.EntityFrameworkCore;

namespace Euroleague.Repository
{
    public class SqlPlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SqlPlayerRepository(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper =  mapper;
            _env = env;
        }
        public async Task<Player> CreatePlayer(EntryPlayerDto playerdTO)
        {

            var player = _mapper.Map<Player>(playerdTO);

            if (playerdTO.logo != null)
            {
                player.ImagePath = await SaveImage(playerdTO.logo);
            }

            _context.Players.Add(player);

            await _context.SaveChangesAsync();

            return player;
        }

       

        public void DeletePlayer(Player player)
        {
            throw new NotImplementedException();
        }

       

        public Player EditPlayer(Player player)
        {
            throw new NotImplementedException();
        }



        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _context.Players.Include(t=>t.Team).ToListAsync();


        }

        public Player GetPlayerById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Player>> GetPlayersByTeamId(int teamId)
        {
            var players= await _context.Players.Include(t=>t.Team).Where(p=>p.TeamId == teamId).ToListAsync();

            return players;
        }


        public async Task<string> SaveImage(IFormFile image)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

            // path for picture
            var imagesPath = Path.Combine(_env.WebRootPath, "images", "players");

            // Provera da li folder postoji, ako ne, kreiraj ga
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            // Putanja do fajla
            var filePath = Path.Combine(imagesPath, uniqueFileName);

            // save picture on server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // return path
            return Path.Combine("images", "players", uniqueFileName);
        }

    }
}
