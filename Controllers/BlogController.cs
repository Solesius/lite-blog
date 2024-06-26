﻿using kw.liteblog.Controllers.ApiModels;
using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kw.liteblog.Controllers;

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

    [HttpPost]
    [Route("create")]
    [Authorize(AuthenticationSchemes = "Custom")]
    public IActionResult AddBlog([FromBody] BlogAddRequest blogAdd)
    {
        var blog = new Blog
        {
            Title = blogAdd.Title,
            Summary = blogAdd.Summary,
            Body = blogAdd.Body
        };

        return Ok(_blogService.CreateNewBlog(blog));
    }

    [HttpPost]
    [Route("update")]
    [Authorize(AuthenticationSchemes = "Custom")]
    public IActionResult EditBlog([FromBody] BlogEditRequest blogAdd)
    {
        var blog = new Blog
        {
            BlogId = blogAdd.BlogId,
            Title = blogAdd.Title,
            Summary = blogAdd.Summary,
            Body = blogAdd.Body
        };

        return Ok(_blogService.UpdateBlog(blog));
    }

    [HttpDelete]
    [Route("delete/{blogId}")]
    [Authorize(AuthenticationSchemes = "Custom")]
    public IActionResult DeleteBlog([FromRoute] int blogId)
    {
        var blog = new Blog { BlogId = blogId };
        
        _blogService.DeleteBlog(blog);
        return Ok();
    }
}
