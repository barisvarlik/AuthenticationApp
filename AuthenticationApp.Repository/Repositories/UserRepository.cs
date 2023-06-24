using AuthenticationApp.Core.Entities;
using AuthenticationApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public async Task AddAsync(User user)
        {
            await _users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _users.Remove(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var cred = await _context.UserCredentials.Where(x => x.Email == email).FirstOrDefaultAsync();
            return await _context.Users.Where(x => x.Id == cred.UserId).FirstOrDefaultAsync();
        }

        public void ChangeCredentials(UserCredentials credentials)
        {
            var cred = _context.UserCredentials.Where(x => x.Email == credentials.Email).First();
            _context.UserCredentials.Update(cred);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _users.FindAsync(id);
        }

        public void Update(User user)
        {
            _users.Update(user);
        }
    }
}
