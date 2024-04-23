using kw.liteblog.Models;

namespace kw.liteblog.Service;

public interface IAdminService
{
    public void UpdateDefaultAdminPassword(string oldPassword, string password);
    public AdminSession EstablishAdminSession(string password);
    public bool SessionValid(string sessionId);
}