using System.Text.RegularExpressions;
using kw.liteblog.Database;
using kw.liteblog.Models;

namespace kw.liteblog.Service;

public partial class BlogService(IDataExtractor<Blog, int, Blog?> blogExtractor) : IBlogService
{
    private readonly IDataExtractor<Blog, int, Blog?> _blogExtractor = blogExtractor;
    public Blog? GetBlog(int blogId)
    {
        return _blogExtractor.ExtractOne(blogId);
    }

    public List<Blog> GetBlogs()
    {
        return _blogExtractor.ExtractMany();
    }

    public Blog? CreateNewBlog(Blog blog)
    {
        RemoveBlogScriptTags(blog);
        return _blogExtractor.InsertOne(blog);
    }

    public Blog? UpdateBlog(Blog blog)
    {
        var currentBlog = GetBlog(blog.BlogId);
        if (currentBlog is null)
            return null;

        RemoveBlogScriptTags(blog);
        return _blogExtractor.UpdateOne(blog);
    }

    private void RemoveBlogScriptTags(Blog blog)
    {
        blog.Title = ScrubHtml(blog.Title);
        blog.Summary = ScrubHtml(blog.Summary);
        blog.Body = ScrubHtml(blog.Body);
    }
    private static string ScrubHtml(string input)
    {
        return MyRegex().Replace(input ?? "", "");
    }

    [GeneratedRegex("<script.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
    private static partial Regex MyRegex();
}