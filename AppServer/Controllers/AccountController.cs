using Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppServer.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    public AccountController() { }

    [HttpPost("verify")]
    public void Verify([FromForm] string email, [FromForm] string password) {
        SLog.Debug(nameof(Verify) + "::{0}::{1}", email, password);

        var acc = Database.Verify(email, password, Utils.GetIPFromRequest(HttpContext));

        if (acc == null) {
            Response.CreateError("Invalid account.");
            return;
        }

        if (Database.IsAccountInUse(acc)) {
            Response.CreateError("Account in use!");
            return;
        }
        
        Response.CreateXML(acc.Export().ToString());
    }

    [HttpPost("register")]
    public void Register([FromForm] string email, [FromForm] string password, [FromForm] string username) {
        SLog.Debug(nameof(Register) + "::{0}::{1}::{2}", email, password, username);


        if (!Database.IsValidEmail(email)) {
            Response.CreateError("Invalid username.");
            return;
        }

        if (!Database.IsValidPassword(password)) {
            Response.CreateError("Invalid password.");
            return;
        }

        if (Database.IsNameTaken(username)) {
            Response.CreateError("Name Taken");
            return;
        }

        var status = Database.RegisterAccount(email, password, username, Utils.GetIPFromRequest(HttpContext));
        if (status != RegisterStatus.Success) {
            Response.CreateError(status.ToString());
            return;
        }

        Response.CreateSuccess();
    }
}
