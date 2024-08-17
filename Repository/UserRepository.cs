using AngularApp.Server.Data;
using AngularApp.Server.Interfaces;
using AngularApp.Server.Models;
using System.ComponentModel.Design;

namespace AngularApp.Server.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
      _context = context;
    }

    public User GetUser(int userId)
    {
      return _context.Users.FirstOrDefault(x => x.UserID == userId);
    }

    public ICollection<User> GetUsers()
    {
      return _context.Users.ToList();
    }

    public bool CreateUser(User user)
    {
      _context.Add(user);
      return Save();
    }

    public bool UpdateUser(User user)
    {
      _context.Update(user);
      return Save();
    }

    public bool DeleteUser(User user)
    {
      _context.Remove(user);
      return Save();
    }

    public bool Save()
    {
      var saved = _context.SaveChanges();
      return saved > 0;
    }

    public bool UserExists(int userId)
    {
      return _context.Users.Any(c => c.UserID == userId);
    }
  }
}
