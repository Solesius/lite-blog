using kw.liteblog.Models;

namespace kw.liteblog.Service;

public interface ISessionService
{
    public bool SessionValid(string sessionId);
}