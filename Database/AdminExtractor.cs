using System.Data.SQLite;

using kw.liteblog.Models;

namespace kw.liteblog.Database;

public class AdminExtractor(string dbPath) : IDataExtractor<BlogAdmin, string, DatabaseResponse>
{
    private readonly string _dbPath = dbPath;
    public List<BlogAdmin> ExtractMany() => [];
    public BlogAdmin? ExtractOne(string username)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
                SELECT 
                    KEY,
                    KEY_SET

                FROM admin_config
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":blogId", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var blogAdmin = new BlogAdmin
                        {
                            Key = reader.GetString(0),
                            IsKeySet = reader.GetBoolean(1)
                        };

                        return blogAdmin;
                    }
                }
            }
        }

        return null;
    }
    public DatabaseResponse UpdateOne(BlogAdmin data)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
            UPDATE admin_config
            SET
                KEY = :password
            WHERE ADMIN_USER = 'admin';
        ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":password", data.Key);
                command.ExecuteNonQuery();
                return new DatabaseResponse { Success = true };
            }
        }
    }

    public void DeleteOne(BlogAdmin valueKey)
    {
        throw new NotImplementedException();
    }

    public DatabaseResponse InsertOne(BlogAdmin data)
    {
        throw new NotImplementedException();
    }

}