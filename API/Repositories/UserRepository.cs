﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<User> AddAsync(User user)
        {
            var result = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            User user = await _appDbContext.Users.FindAsync(id);
            if (user != null)
            { 
                _appDbContext.Users.Remove(user);
                await _appDbContext.SaveChangesAsync();            
            }
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _appDbContext.Users
                .Include(c => c.Genre)
                .ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _appDbContext.Users
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _appDbContext.Users
                .Include(c => c.Genre)
                //.ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task<IEnumerable<User>> SearchAsync(string name, string genre, int? age)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            _appDbContext.Entry(user).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();                         
        }
    }
}
