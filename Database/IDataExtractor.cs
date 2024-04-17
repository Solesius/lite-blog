namespace kw.liteblog.Database;

public interface IDataExtractor<T,U> 
{
    public T? ExtractOne(U valueKey);
    public List<T> ExtractMany();
}