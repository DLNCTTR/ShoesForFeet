using System.Linq;
using ShoesForFeet.Data;
using ShoesForFeet.Models;

namespace ShoesForFeet.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}