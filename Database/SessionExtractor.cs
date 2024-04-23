using System.Data.SQLite;

using kw.liteblog.Models;

namespace kw.liteblog.Database;

public class SessionExtractor(string dbPath) : IDataExtractor<AdminSession, string, DatabaseResponse>
{
    private readonly string _dbPath = dbPath;
    public DatabaseResponse InsertOne(AdminSession session)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
            INSERT INTO admin_session (
                SESSION_ID,
                SESSION_TIME
            ) VALUES (
                :sessionId,
                strftime('%s', 'now')
            );
        ";
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":sessionId", session.SessionId);

                command.ExecuteNonQuery();
            }
        }

        return new DatabaseResponse { };
    }


    public AdminSession? ExtractOne(string valueKey)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
                SELECT 
                    SESSION_ID,
                    SESSION_TIME

                FROM admin_session
                WHERE SESSION_ID = :sessionId;
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":sessionId", valueKey);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var adminSession = new AdminSession
                        {
                            SessionId = reader.GetString(0),
                            SessionTime = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(1)).ToLocalTime().LocalDateTime,
                        };

                        return adminSession;
                    }
                }
            }
        }

        return null;
    }
    public List<AdminSession> ExtractMany() => [];

    public void DeleteOne(AdminSession value)
    {
        throw new NotImplementedException();
    }

    //support refresh in future
    public DatabaseResponse UpdateOne(AdminSession value)
    {
        throw new NotImplementedException();
    }

}
