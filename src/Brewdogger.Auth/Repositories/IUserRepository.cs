using System.Collections.Generic;
using Brewdogger.Auth.Entities;

namespace Brewdogger.Auth.Repositories
{
    public interface IUserRepository
    {
        ICollection<User> FindAllUsers();
        User FindUserById(int id);
        User FindUserByUsername(string username);
        void SaveUser(User user);
        void UpdateUser(User user, User updatedUser);
        void DeleteUser(User user);
    }
}