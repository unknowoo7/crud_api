using AngularApp.Server.Models;

namespace AngularApp.Server.Interfaces
{
  public interface IUserRepository
  {
    ICollection<User> GetUsers();
    User GetUser(int userId);
    bool CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(User user);
    bool UserExists(int userId);
  }
}
