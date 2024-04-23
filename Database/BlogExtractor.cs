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
                    AUTHOR,
                    POST_DATE,
                    SUMMARY,
                    BODY

                FROM BLOG
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
                    AUTHOR,
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
                AUTHOR,
                POST_DATE,
                SUMMARY,
                BODY
            ) VALUES (
                :title,
                :author,
                strftime('%s', 'now'),
                :summary,
                :body
            );

            SELECT 
                BLOG_ID,
                TITLE,
                AUTHOR,
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
                command.Parameters.AddWithValue(":author", data.Author);
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
                Author = reader.GetString(2),
                PostDate = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(3)).ToLocalTime().LocalDateTime,
                Summary = reader.GetString(4),
                Body = reader.GetString(5)
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
                Author = reader.GetString(2),
                PostDate = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(3)).ToLocalTime().LocalDateTime,
                Summary = reader.GetString(4),
                Body = reader.GetString(5)
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
                AUTHOR = :author,
                POST_DATE = strftime('%s', 'now'),
                SUMMARY = :summary,
                BODY = :body
            WHERE BLOG_ID = :blogId;
        ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":title", blog.Title);
                command.Parameters.AddWithValue(":author", blog.Author);
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