using AutoMapper;
using AngularApp.Server.Dto;
using AngularApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AngularApp.Server.Models;
using AngularApp.Server.Repository;

namespace AngularApp.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : Controller
  {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;

    public UserController(
      IMapper mapper,
      IUserRepository userRepository,
      ICompanyRepository companyRepository
    )
    {
      _mapper = mapper;
      _userRepository = userRepository;
      _companyRepository = companyRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    public IActionResult Get() 
    { 
      return Ok(_userRepository.GetUsers());
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    public IActionResult GetUser(int userId)
    {
      return Ok(_userRepository.GetUser(userId));
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateUser([FromQuery] int companyId, [FromBody] User userCreate)
    {
      if (userCreate == null) 
      {
        return BadRequest(ModelState);
      }

      var user = _userRepository
                .GetUsers()
                .Where(o => o.UserName.Trim().ToLower() == userCreate.UserName.Trim().ToLower())
                .FirstOrDefault();

      if (user != null)
      {
        ModelState.AddModelError("", "Already exists");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      userCreate.CompanyID = _companyRepository.GetCompanyId(companyId).CompanyID;

      if (!_userRepository.CreateUser(userCreate))
      {
        ModelState.AddModelError("", "Something went wrong while saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }

    [HttpPut("{userId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateUser(int userId, [FromBody] User updateUser)
    {
      if (updateUser == null)
      {
        return BadRequest(ModelState);
      }

      if (userId != updateUser.UserID)
      {
        return BadRequest(ModelState);
      }

      if (!_userRepository.UserExists(userId))
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      updateUser.UserID = userId;

      if (!_userRepository.UpdateUser(updateUser))
      {
        ModelState.AddModelError("", "Something went wrong updating");
        return StatusCode(500, ModelState);
      }

      return NoContent();
    }

    [HttpDelete("{userId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int userId)
    {
      if (!_userRepository.UserExists(userId))
      {
        return NotFound();
      }

      var userToDelete = _userRepository.GetUser(userId);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!_userRepository.DeleteUser(userToDelete))
      {
        ModelState.AddModelError("", "Something went wrong deleting");
      }

      return NoContent();
    }
  }
}
