using ShoesForFeet.Models;
using System.Collections.Generic;

namespace ShoesForFeet.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers(); // Retrieve all users
        User GetUserById(int id); // Retrieve a user by ID
        void AddUser(User user); // Add a new user
        void UpdateUser(User user); // Update an existing user
        void DeleteUser(int id); // Delete a user by ID
    }
}