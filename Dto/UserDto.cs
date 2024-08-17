using AngularApp.Server.Models;

namespace AngularApp.Server.Dto
{
  public class UserDto
  {
    public int UserID { get; set; }
    public string UserName { get; set; } = "";
    public string UserEmail { get; set; } = "";
    public int CompanyID { get; set; }
  }
}
