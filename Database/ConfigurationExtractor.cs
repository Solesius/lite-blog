using System.Data.SQLite;

using kw.liteblog.Models;

namespace kw.liteblog.Database;

public class ConfigurationExtractor(string dbPath) : IDataExtractor<AppConfig, string, DatabaseResponse>
{
    private readonly string _dbPath = dbPath;
    public List<AppConfig> ExtractMany() => [];
    public AppConfig? ExtractOne(string configKey)
    {
        using (var connection = new SQLiteConnection(_dbPath))
        {
            connection.Open();
            string sql = @"
                SELECT 
                    CONFIG_KEY,
                    CONFIG_VALUE

                FROM app_config
                WHERE CONFIG_KEY = :configKey
            ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue(":configKey", configKey);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var config = new AppConfig
                        {
                            ConfigKey = reader.GetString(0),
                            ConfigValue = reader.GetString(1)
                        };

                        return config;
                    }
                }
            }
        }

        return null;
    }

    public DatabaseResponse UpdateOne(AppConfig data)
    {
        throw new NotImplementedException();
    }

    public void DeleteOne(AppConfig valueKey)
    {
        throw new NotImplementedException();
    }

    public DatabaseResponse InsertOne(AppConfig data)
    {
        throw new NotImplementedException();
    }

}