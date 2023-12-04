using Common;
using Microsoft.AspNetCore.Mvc;

namespace AppServer.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    public AccountController() { }

    [HttpPost("verify")]
    public void Verify([FromForm] string email, [FromForm] string password) {
        SLog.Debug(nameof(Verify) + "{0}, {1}", email, password);
    }

    [HttpPost("register")]
    public void Register([FromForm] string email, [FromForm] string password, [FromForm] string username) {
        SLog.Debug(nameof(Register) + "{0}, {1}, {2}", email, password, username);
    }
}
