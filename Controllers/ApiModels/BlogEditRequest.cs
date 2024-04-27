namespace kw.liteblog.Controllers.ApiModels;

public class BlogEditRequest
{
    public required int BlogId { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public required string Body { get; set; }
}