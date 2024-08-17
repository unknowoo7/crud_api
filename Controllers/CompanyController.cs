using AngularApp.Server.Dto;
using AngularApp.Server.Interfaces;
using AngularApp.Server.Models;
using AngularApp.Server.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AngularApp.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CompanyController : Controller
  {
    private readonly IMapper _mapper;
    private readonly ICompanyRepository _companyRepository;

    public CompanyController(
      IMapper mapper,
      ICompanyRepository companyRepository
    )
    {
      _mapper = mapper;
      _companyRepository = companyRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyDto>))]
    public IActionResult Get()
    {
      return Ok(_companyRepository.GetCompanies());
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType(200, Type = typeof(CompanyDto))]
    [ProducesResponseType(400)]
    public IActionResult GetCompany(int companyId)
    {
      return Ok(_companyRepository.GetCompanyId(companyId));
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCompany([FromQuery] int companyId, [FromBody] Company companyCreate)
    {
      if (companyCreate == null)
      {
        return BadRequest(ModelState);
      }

      var company = _companyRepository
                .GetCompanies()
                .Where(o => o.CompanyName.Trim().ToLower() == companyCreate.CompanyName.Trim().ToLower())
                .FirstOrDefault();

      if (company != null)
      {
        ModelState.AddModelError("", "Already exists");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!_companyRepository.CreateCompany(companyCreate))
      {
        ModelState.AddModelError("", "Something went wrong while saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }

    [HttpPut("{companyId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCompany(int companyId, [FromBody] Company updateCompany)
    {
      if (updateCompany == null)
      {
        return BadRequest(ModelState);
      }

      if (companyId != updateCompany.CompanyID)
      {
        return BadRequest(ModelState);
      }

      if (!_companyRepository.CompanyExists(companyId))
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      updateCompany.CompanyID = companyId;

      if (!_companyRepository.UpdateCompany(updateCompany))
      {
        ModelState.AddModelError("", "Something went wrong updating");
        return StatusCode(500, ModelState);
      }

      return NoContent();
    }

    [HttpDelete("{companyId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int companyId)
    {
      if (!_companyRepository.CompanyExists(companyId))
      {
        return NotFound();
      }

      var companyToDelete = _companyRepository.GetCompanyId(companyId);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!_companyRepository.DeleteCompany(companyToDelete))
      {
        ModelState.AddModelError("", "Something went wrong deleting");
      }

      return NoContent();
    }
  }
}
