namespace AngularApp.Server.Models
{
  public class User
  {
    public int UserID { get; set; }
    public string UserName { get; set; } = ""; 
    public string UserEmail { get; set; } = ""; 
    public int CompanyID { get; set; }
  }
}
