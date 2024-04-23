using kw.liteblog.Controllers.ApiModels;
using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Mvc;

namespace net.Controllers;

[ApiController]
[Route("api/admin")]

public class AdministrationController(
    ILogger<AdministrationController> logger,
    IConfiguration configuration) : ControllerBase
{
    private readonly ILogger<AdministrationController> _logger = logger;
    private readonly IAdminService _adminService = new AdminService(
        new AdminExtractor("Data Source=app.db"),
        new SessionExtractor("Data Source=app.db"),
        configuration);

    [HttpPost]
    [Route("session/create")]
    public IActionResult CreateAdminSession([FromBody] SessionRequest sessionRequest)
    {

        return Ok(_adminService.EstablishAdminSession(sessionRequest.Key));
    }

    [HttpPost]
    [Route("session/valid")]
    public IActionResult ValidateSession([FromBody] SessionCheckRequest sessionCheckRequest)
    {
        return Ok(ValidateSession(sessionCheckRequest.SessionId));
    }

    [HttpPost]
    [Route("password/update")]

    public IActionResult GetBlog([FromBody] PasswordUpdateRequest passwordUpdateRequest)
    {
        if (!Request.Headers.TryGetValue("Session-Token", out var headerValues) || headerValues.Count == 0)
        {
            return Forbid();
        }

        var sessionId = headerValues[0];
        if (ValidateSession(sessionId))
        {
            _adminService.UpdateDefaultAdminPassword(passwordUpdateRequest.ConfirmKey, passwordUpdateRequest.NewKey);
            return Ok();
        }
        else
        {
            return Forbid();
        }
    }

    private bool ValidateSession(string id)
    {
        return _adminService.SessionValid(id);
    }
}
