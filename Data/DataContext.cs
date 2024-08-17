using AngularApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
  }
}
