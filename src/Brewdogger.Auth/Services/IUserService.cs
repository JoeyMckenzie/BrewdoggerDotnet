using System.Collections.Generic;
using Brewdogger.Auth.Entities;
using Brewdogger.Auth.Models;

namespace Brewdogger.Auth.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        ICollection<User> GetAll();
        User GetUserById(int id);
        User CreateUser(UserDto userDto);
        string CreateToken(User user);
        void UpdateUser(UserDto userDto, string password = null);
        void DeleteUser(int id);
    }
}