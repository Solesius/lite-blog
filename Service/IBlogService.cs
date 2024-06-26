using kw.liteblog.Models;

namespace kw.liteblog.Service;

public interface IBlogService
{
    public Blog? GetBlog(int blogId);
    public List<Blog> GetBlogs();
    public Blog? CreateNewBlog(Blog blog);
    public Blog? UpdateBlog(Blog blog);
    public void DeleteBlog(Blog blog);
}