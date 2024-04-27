using kw.liteblog.Controllers.ApiModels;
using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "Custom")]
    public IActionResult ValidateSession()
    {
        return Ok(true);
    }

    [HttpPost]
    [Route("password/update")]
    [Authorize(AuthenticationSchemes = "Custom")]
    public IActionResult GetBlog([FromBody] PasswordUpdateRequest passwordUpdateRequest)
    {
        _adminService.UpdateDefaultAdminPassword(passwordUpdateRequest.ConfirmKey, passwordUpdateRequest.NewKey);
        return Ok();
    }
}
