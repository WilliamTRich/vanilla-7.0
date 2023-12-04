using Common;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AppServer.Controllers;

[ApiController]
[Route("char")]
public class CharacterController : ControllerBase
{
    public CharacterController() { }

    [HttpPost("list")]
    public void List([FromForm] string email, [FromForm] string password) {
        SLog.Debug(nameof(List) + "::{0}::{1}", email, password);

        var acc = Database.Verify(email, password, Utils.GetIPFromRequest(HttpContext));

        if (acc == null) {
            Response.CreateError("Invalid account.");
            return;
        }

        if (Database.IsAccountInUse(acc)) {
            Response.CreateError("Account in use!");
            return;
        }

        var data = new XElement("Chars");
        var servers = new XElement("Servers");
        servers.Add(new XElement("Server",
                new XElement("Name", Settings.ServerName),
                new XElement("DNS", Settings.Address),
                new XElement("Port", Settings.Ports[1]),
                new XElement("Lat", 0.0),
                new XElement("Long", 0.0),
                new XElement("Usage", 0),
                new XElement("AdminOnly", Settings.AdminOnly))
            );

        data.Add(new XAttribute("nextCharId", acc.NextCharId));
        data.Add(new XAttribute("maxNumChars", acc.MaxNumChars));
        data.Add(acc.Export());
        data.Add(Database.GetNews(acc));
        data.Add(new XElement("OwnedSkins", string.Join(",", acc.OwnedSkins)));
        foreach (var charId in acc.AliveChars)
        {
            var character = Database.LoadCharacter(acc, charId);
            var export = character.Export();
            export.Add(new XAttribute("id", charId));
            data.Add(export);
        }

        data.Add(servers);

        Response.CreateXML(data.ToString());
    }
}
