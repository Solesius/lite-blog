using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Mvc;

namespace net.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController(ILogger<BlogController> logger) : ControllerBase
{
    private readonly ILogger<BlogController> _logger = logger;
    private readonly IBlogService _blogService = new BlogService(new BlogExtractor("Data Source=app.db"));

    [HttpGet]
    [Route("fetch/{id}")]
    public IActionResult GetBlog([FromRoute] int id)
    {
        _logger.LogInformation("Fetching blog: {0}", id);
        return Ok(_blogService.GetBlog(id));
    }

    [HttpGet]
    [Route("list")]
    public IActionResult ListBlogs()
    {
        return Ok(_blogService.GetBlogs());
    }
}
