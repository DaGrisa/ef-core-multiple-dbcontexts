namespace Common
{
  public interface IDbContextProvider<T>
  {
    T CreateContext();
  }
}