using Common;
using System.Net;
namespace AppServer;

public class Utils {
    public static string GetIPFromRequest(HttpContext context) {
        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
