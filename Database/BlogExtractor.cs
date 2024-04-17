using System.Data.SQLite;
using kw.liteblog.Models;

namespace kw.liteblog.Database;

public class BlogExtractor : IDataExtractor<Blog, int>
{
    private readonly string _dbPath;
    public BlogExtractor(string dbPath)
    {
        this._dbPath = dbPath;
    }

    public List<Blog> ExtractMany()
    {
        return new();
    }

    public Blog? ExtractOne(int blogId)
    {
        Blog? blog = null;
        using (var connection = new SQLiteConnection(this._dbPath))
        {
            connection.Open();
            string sql = @"
                SELECT 
                    BLOG_ID,
                    TITLE,
                    AUTHOR,
                    POST_DATE,
                    SUMMARY,
                    BODY

                FROM BLOG
                WHERE BLOG_ID = :blogId
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":blogId", blogId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        blog = new Blog
                        {
                            BlogId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            PostDate = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(3)).ToLocalTime().LocalDateTime,
                            Summary = reader.GetString(4),
                            Body = reader.GetString(5)
                        };
                    }
                }
            }
        }

        return blog;
    }
}