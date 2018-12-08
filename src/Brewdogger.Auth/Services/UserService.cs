using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Brewdogger.Auth.Entities;
using Brewdogger.Auth.Exceptions;
using Brewdogger.Auth.Helpers;
using Brewdogger.Auth.Models;
using Brewdogger.Auth.Repositories;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Brewdogger.Auth.Services
{
    /// <summary>
    /// User service to CRUD operations and user authentication. When creating users, passwords are hashed
    /// and salted using HMAC SHA512 via <c>CreatePasswordHash</c> and verified by matching salts and hashes
    /// during initial user creation.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ISecretHelper _secretHelper;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(ISecretHelper secretHelper, IMapper mapper, IUserRepository repository)
        {
            _secretHelper = secretHelper;
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// Authenticate the user attempting to login
        /// </summary>
        /// <param name="username">Username input</param>
        /// <param name="password">Raw password input</param>
        /// <returns>User object with associated properties</returns>
        /// <exception cref="InvalidCredentialException">Throws if username or password are invalidated</exception>
        /// <exception cref="UserNotFoundException">Throws if username does not exist in database</exception>
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new InvalidCredentialException("Username or password is null");

            var user = _repository.FindUserByUsername(username);
            
            if (user == null)
                throw new UserNotFoundException($"No users found");
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidCredentialException($"Password is incorrect for user [{user.Username}]");

            return user;
        }

        /// <summary>
        /// Get a collection of all user entities
        /// </summary>
        /// <returns>Collection of user entities</returns>
        /// <exception cref="UserNotFoundException">Throws if no users in database</exception>
        public ICollection<User> GetAll()
        {
            var users = _repository.FindAllUsers();
            
            if (users == null || users.Count == 0)
                throw new UserNotFoundException("No users found");

            return users;
        }

        /// <summary>
        /// Get a user by primary key
        /// </summary>
        /// <param name="id">Primary integer ID</param>
        /// <returns>User entity</returns>
        /// <exception cref="UserNotFoundException">Throws if no user is found in database</exception>
        public User GetUserById(int id)
        {
            var user = _repository.FindUserById(id);
            
            if (user == null)
                throw new UserNotFoundException("No user found");

            return user;
        }

        /// <summary>
        /// Create a new user from the User DTO and password using simple security to hash and salt
        /// </summary>
        /// <param name="userDto">User DTO sent from request body</param>
        /// <param name="password">User password sent from request body</param>
        /// <returns>Newly created user object</returns>
        /// <exception cref="InvalidCredentialException">Throw if password is null or empty</exception>
        /// <exception cref="UserExistException">Throw is username is already taken</exception>
        /// <exception cref="UserCreationException">Throw if there was an issue saving the user</exception>
        public User CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new InvalidCredentialException("Password cannot be null or empty");
            
            if (_repository.FindUserByUsername(userDto.Username) != null)
                throw new UserExistException($"Username already exists: [{userDto.Username}]");

            var newUser = _mapper.Map<User>(userDto);
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            if (passwordHash != null && passwordHash.Length == 64 &&
                    passwordSalt != null && passwordSalt.Length == 128)
            {
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                try
                {
                    _repository.SaveUser(newUser);
                }
                catch (Exception e)
                {
                    Log.Error("UserService::CreateUser - User not created. Reason [{0}]", e.Message);
                    throw new UserCreationException("User not created", e);
                }

                Log.Information("UserService::CreateUser - User successfully created [{0}]", newUser.Username);
                return newUser;
            }
            
            throw new UserCreationException("User not created, error creating password hash and salt");
        }

        /// <summary>
        /// Generates a JSON web token for the the user after authenticating from the UserController
        /// TODO: Add scopes to token
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>JWT with user information</returns>
        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_secretHelper.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email), 
                    new Claim(ClaimTypes.UserData, user.Username)
                }),
                Issuer = "https://localhost:6001", // TODO: Modify once DNS is set
                Audience = "https://localhost:5001",
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public void UpdateUser(UserDto userDto, string password = null)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Helper method to create stored passwords using HMAC SHA512, and validates the password before salting and hashing
        /// </summary>
        /// <param name="password">Raw text password</param>
        /// <param name="passwordHash">Returned password hash, if successful</param>
        /// <param name="passwordSalt">Returned password salt, if successful</param>
        /// <exception cref="ArgumentNullException">Thrown for null input password</exception>
        /// <exception cref="ArgumentException">Thrown if password is empty</exception>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("Password cannot be null");
            
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string.");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        /// <summary>
        /// Helper to verify password of user by validating the salt and hash stored in user table
        /// </summary>
        /// <param name="password">Raw text password</param>
        /// <param name="storedHash">Returned password hash, if successful</param>
        /// <param name="storedSalt">Returned password salt, if successful</param>
        /// <returns>Boolean status of verification attempt</returns>
        /// <exception cref="ArgumentNullException">Throw for null input password</exception>
        /// <exception cref="ArgumentException">Throw if password is empty</exception>
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("Password is null");
            
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string");
            
            if (storedHash.Length != 64) 
                throw new ArgumentException($"Invalid length of password hash (64 bytes expected): {storedHash}");
            
            if (storedSalt.Length != 128) 
                throw new ArgumentException($"Invalid length of password salt (128 bytes expected: {storedSalt}");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
 
            return true;
        }
    }
}