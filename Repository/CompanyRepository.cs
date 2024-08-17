using AngularApp.Server.Data;
using AngularApp.Server.Interfaces;
using AngularApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Repository
{
  public class CompanyRepository : ICompanyRepository
  {
    private readonly DataContext _context;

    public CompanyRepository(DataContext context)
    {
      _context = context;
    }

    public bool CreateCompany(Company company)
    {
      _context.Add(company);
      return Save();
    }

    public bool DeleteCompany(Company company)
    {
      _context.Remove(company);
      return Save();
    }

    public ICollection<Company> GetCompanies()
    {
      return _context.Companies.ToList();
    }

    public Company GetCompanyId(int companyId)
    {
      return _context.Companies.FirstOrDefault(c => c.CompanyID == companyId);
    }

    public bool UpdateCompany(Company company)
    {
      _context.Update(company);
      return Save();
    }

    public bool CompanyExists(int companyId)
    {
      return _context.Companies.Any(c => c.CompanyID == companyId);
    }
    
    public bool Save()
    {
      var saved = _context.SaveChanges();
      return saved > 0;
    }
  }
}
