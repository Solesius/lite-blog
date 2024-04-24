namespace kw.liteblog.Controllers.ApiModels;

public class BlogAddRequest
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
}