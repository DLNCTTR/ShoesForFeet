using ShoesForFeet.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoesForFeet.Data
{
    public class UserRepository : IUserRepository
    {
        // Simulating a database with a static list
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Tom", Password = "1234" },
            new User { Id = 2, Name = "JaneSmith", Password = "12345" }
        };

        public IEnumerable<User> GetAllUsers()
        {
            return _users; // Return all users
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id); // Find user by ID
        }

        public void AddUser(User user)
        {
            user.Id = _users.Count + 1; // Assign a new unique ID
            _users.Add(user); // Add user to the list
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                // Update user details
                existingUser.Name = user.Name;
                existingUser.Password = user.Password;
            }
        }

        public void DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user); // Remove user from the list
            }
        }
    }
}