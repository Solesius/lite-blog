using System.Security.Claims;
using System.Text.Encodings.Web;
using kw.liteblog.Database;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace kw.liteblog.Controllers.Filters;

[method: Obsolete]
public class SessionHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    IConfiguration configuration,
    UrlEncoder encoder,
    ISystemClock clock) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private readonly IAdminService _adminService = new AdminService(
        new AdminExtractor("Data Source=app.db"),
        new SessionExtractor("Data Source=app.db"),
        configuration
    );

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Context.Request.Headers["X-SESSION-ID"].FirstOrDefault();
        var failTask = Task.FromResult(AuthenticateResult.Fail("Invalid Session Id"));

        if (authorizationHeader is null) return failTask;

        if (_adminService.SessionValid(authorizationHeader))
        {
            var claims = new[] { new Claim("session-valid", "1") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        else return failTask;
    }
}