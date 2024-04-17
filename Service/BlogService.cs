using kw.liteblog.Database;
using kw.liteblog.Models;

namespace kw.liteblog.Service;

public class BlogService(IDataExtractor<Blog,int> blogExtractor) : IBlogService
{
    private readonly IDataExtractor<Blog,int> _blogExtractor = blogExtractor;
    public Blog? GetBlog(int blogId)
    {
        return _blogExtractor.ExtractOne(blogId);
    }
}