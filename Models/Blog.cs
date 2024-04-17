namespace kw.liteblog.Models;

public class Blog
{
    public int BlogId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public DateTime PostDate { get; set; }
    public string? Summary { get; set; }
    public string? Body { get; set; }
}