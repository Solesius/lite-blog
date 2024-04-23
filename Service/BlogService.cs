using kw.liteblog.Database;
using kw.liteblog.Models;

namespace kw.liteblog.Service;

public class BlogService(IDataExtractor<Blog, int, Blog?> blogExtractor) : IBlogService
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

    public Blog? AddBlog(Blog blog)
    {
        return _blogExtractor.InsertOne(blog);
    }
}