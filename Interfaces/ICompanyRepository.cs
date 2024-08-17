using AngularApp.Server.Models;

namespace AngularApp.Server.Interfaces
{
  public interface ICompanyRepository
  {
    ICollection<Company> GetCompanies();
    Company GetCompanyId(int companyId);
    bool CreateCompany(Company company);
    bool UpdateCompany(Company company);
    bool DeleteCompany(Company company);
    bool CompanyExists(int companyId);
  }
}
