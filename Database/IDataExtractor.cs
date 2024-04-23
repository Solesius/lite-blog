namespace kw.liteblog.Database;

public interface IDataExtractor<T>
{
    public void DeleteOne(T value);
}
public interface IDataExtractor<T, U> : IDataExtractor<T>
{
    public T? ExtractOne(U valueKey);
    public List<T> ExtractMany();
}
public interface IDataExtractor<T, U, V> : IDataExtractor<T, U>
{
    public V InsertOne(T value);
    public V UpdateOne(T value);
}