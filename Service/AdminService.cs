using System.Security.Cryptography;
using System.Text;
using kw.liteblog.Database;
using kw.liteblog.Models;

namespace kw.liteblog.Service;

public class AdminService(
    IDataExtractor<BlogAdmin, string, DatabaseResponse> adminExtractor,
    IDataExtractor<AdminSession, string, DatabaseResponse> sessionExtractor,
    IConfiguration configuration) : IAdminService
{
    private readonly IDataExtractor<BlogAdmin, string, DatabaseResponse> _adminExtractor = adminExtractor;
    private readonly IDataExtractor<AdminSession, string, DatabaseResponse> _sessionExtractor = sessionExtractor;
    private readonly IConfiguration _config = configuration;
    private readonly ILogger<AdminService> _logger = new LoggerFactory().CreateLogger<AdminService>();

    public void UpdateDefaultAdminPassword(string oldPassword, string password)
    {
        var encKey = _config["SESSION_KEY"];
        if (encKey is not null)
        {
            var pass = CreateKeyHash(Encoding.UTF8.GetString(Convert.FromBase64String(password)), encKey);

            var current = _adminExtractor.ExtractOne("admin");
            if (current is not null)
            {
                if (current.Key is not null && current.Key.Equals(CreateKeyHash(Encoding.UTF8.GetString(Convert.FromBase64String(oldPassword)), encKey)))
                {
                    var update = new BlogAdmin { Key = pass };
                    _adminExtractor.UpdateOne(update);
                }
            }
        }
    }

    public AdminSession EstablishAdminSession(string password)
    {
        //todo move to db?
        var encKey = _config["SESSION_KEY"];
        if (encKey is not null && !string.IsNullOrEmpty(password))
        {
            var blogAdmin = _adminExtractor.ExtractOne("admin");
            if (blogAdmin is not null)
            {
                var hashPass = CreateKeyHash(Encoding.UTF8.GetString(Convert.FromBase64String(password)), encKey);

                if (blogAdmin.Key is not null && blogAdmin.Key.Equals(hashPass))
                {
                    var session = new AdminSession { SessionId = GenerateSessionId() };
                    _sessionExtractor.InsertOne(session);
                    return session;
                }
            }
        }

        return new AdminSession { };
    }

    public bool SessionValid(string sessionId)
    {
        //did we find an administration session that is not expired. 
        var session = _sessionExtractor.ExtractOne(sessionId);
        var sessionValid = session is not null &&
                           session.SessionTime.AddMinutes(60) > DateTime.Now;
        return sessionValid;

    }

    private static string GenerateSessionId()
    {
        using var rng = RandomNumberGenerator.Create();
        var data = new byte[64];
        rng.GetBytes(data);
        return Convert.ToBase64String(data);
    }

    private static string CreateKeyHash(string value, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
        return BitConverter.ToString(hash).Replace("-", "");
    }
}