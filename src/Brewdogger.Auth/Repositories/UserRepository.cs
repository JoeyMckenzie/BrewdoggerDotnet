using System.Collections.Generic;
using System.Linq;
using Brewdogger.Auth.Entities;
using Brewdogger.Auth.Persistence;

namespace Brewdogger.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BrewdoggerAuthDbContext _context;

        public UserRepository(BrewdoggerAuthDbContext context)
        {
            _context = context;
        }

        public ICollection<User> FindAllUsers()
        {
            return _context.Users.ToList();
        }
        
        public User FindUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User FindUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user, User updatedUser)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}