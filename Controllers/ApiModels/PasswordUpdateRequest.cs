namespace kw.liteblog.Controllers.ApiModels
{
    public class PasswordUpdateRequest
    {
        public required string ConfirmKey { get; set; }
        public required string NewKey { get; set; }
    }

}