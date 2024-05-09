using System.Data.SQLite;

using kw.liteblog.Models;

namespace kw.liteblog.Database;

public class BlogExtractor(string dbPath) : IDataExtractor<Blog, int, Blog?>
{
    private readonly string _dbPath = dbPath;
    public List<Blog> ExtractMany()
    {
        List<Blog> blogs = new();

        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string query = @"
                SELECT 
                    BLOG_ID,
                    TITLE,
                    POST_DATE,
                    SUMMARY,
                    COALESCE(PINNED,0)

                FROM BLOG
                ORDER BY 
					PINNED = 1 DESC, 
					POST_DATE DESC
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    blogs = ExtractBlogsWithReader(reader);
                }
            }
        }

        return blogs;
    }

    public Blog? ExtractOne(int blogId)
    {
        Blog? blog = null;
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string query = @"
                SELECT 
                    BLOG_ID,
                    TITLE,
                    POST_DATE,
                    SUMMARY,
                    BODY

                FROM BLOG
                WHERE BLOG_ID = :blogId
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(":blogId", blogId);

                using (var reader = command.ExecuteReader())
                {
                    blog = ExtractBlogWithReader(reader);
                }
            }
        }

        return blog;
    }

    public Blog? InsertOne(Blog data)
    {
        Blog? insertedBlog = null;
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string query = @"
            INSERT INTO BLOG (
                TITLE,
                POST_DATE,
                SUMMARY,
                BODY
            ) VALUES (
                :title,
                strftime('%s', 'now'),
                :summary,
                :body
            );

            SELECT 
                BLOG_ID,
                TITLE,
                POST_DATE,
                SUMMARY,
                BODY

            FROM BLOG
            WHERE rowid = last_insert_rowid();
        ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = query;
                command.Parameters.AddWithValue(":title", data.Title);
                command.Parameters.AddWithValue(":summary", data.Summary);
                command.Parameters.AddWithValue(":body", data.Body);

                using (var reader = command.ExecuteReader())
                {
                    insertedBlog = ExtractBlogWithReader(reader);
                }
            }
        }

        return insertedBlog;
    }

    public List<Blog> ExtractBlogsWithReader(SQLiteDataReader reader)
    {
        var blogs = new List<Blog>();

        while (reader.Read())
        {
            var blog = new Blog
            {
                BlogId = reader.GetInt32(0),
                Title = reader.GetString(1),
                PostDate = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(2)).ToLocalTime().LocalDateTime,
                Summary = reader.GetString(3),
                IsPinned = reader.GetInt32(4) > 0
            };

            blogs.Add(blog);
        }

        return blogs;
    }

    public Blog? ExtractBlogWithReader(SQLiteDataReader reader)
    {
        if (reader.Read())
        {
            var blog = new Blog
            {
                BlogId = reader.GetInt32(0),
                Title = reader.GetString(1),
                PostDate = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(2)).ToLocalTime().LocalDateTime,
                Summary = reader.GetString(3),
                Body = reader.GetString(4)
            };

            return blog;
        }

        return null;
    }

    public Blog? UpdateOne(Blog blog)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
            UPDATE BLOG
            SET
                TITLE = :title,
                POST_DATE = strftime('%s', 'now'),
                SUMMARY = :summary,
                BODY = :body
            WHERE BLOG_ID = :blogId;
        ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":title", blog.Title);
                command.Parameters.AddWithValue(":postDate", blog.PostDate);
                command.Parameters.AddWithValue(":summary", blog.Summary);
                command.Parameters.AddWithValue(":body", blog.Body);
                command.Parameters.AddWithValue(":blogId", blog.BlogId);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    return blog;
                }
            }
        }

        return null;
    }
    public void DeleteOne(Blog blog)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
                DELETE FROM BLOG
                WHERE BLOG_ID = :blogId;
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":blogId", blog.BlogId);

                command.ExecuteNonQuery();
            }
        }
    }
}