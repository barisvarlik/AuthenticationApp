using AuthenticationApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmail(string email);
        void ChangeCredentials(UserCredentials credentials);
    }
}
