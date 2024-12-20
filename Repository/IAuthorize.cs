﻿using Euroleague.Models;

namespace Euroleague.Repository
{
    public interface IAuthorize
    {
        Task<string> GenerateToken(User user); 
        Task<User> SignIn(string username, string password, string email); 
        Task<User> LogIn(string username, string password);
    }
}
