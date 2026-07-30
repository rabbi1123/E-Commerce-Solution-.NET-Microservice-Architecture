using Microsoft.AspNetCore.Mvc;
using User.API.Services;
using UserEntity = User.API.Models.User;

namespace User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserEntity>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserEntity>> GetById(Guid userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserEntity>> Create([FromBody] UserEntity user)
    {
        if (user.UserId == Guid.Empty)
        {
            return BadRequest("UserId is required.");
        }

        var created = await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { userId = created.UserId }, created);
    }

    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<UserEntity>> Update(Guid userId, [FromBody] UserEntity user)
    {
        var updated = await _userService.UpdateAsync(userId, user);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var deleted = await _userService.DeleteAsync(userId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
