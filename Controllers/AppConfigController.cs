using kw.liteblog.Controllers.ApiModels;
using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace net.Controllers;

[ApiController]
[Route("api/app/config")]

public class AppConfigController(ILogger<AdministrationController> logger) : ControllerBase
{
    private readonly ILogger<AdministrationController> _logger = logger;
    private readonly IDataExtractor<AppConfig,string,DatabaseResponse> _configExtractor = new ConfigurationExtractor("Data Source=app.db");

    [HttpPost]
    [Route("{configName}")]
    public IActionResult GetAppConfig([FromRoute] string configName)
    {
        return Ok(_configExtractor.ExtractOne(configName));
    }
}
