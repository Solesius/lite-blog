using Microsoft.AspNetCore.Mvc;

namespace net.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly ILogger<BlogController> _logger;

    public BlogController(ILogger<BlogController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("/list")]
    public IActionResult ListBlog()
    {
        return Ok("");
    }
}
