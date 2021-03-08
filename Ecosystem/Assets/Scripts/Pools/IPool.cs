namespace Pools
{
  public interface IPool<T>
  {
    /// <summary>
    ///   Retrieves a pooled item. Creates a new one if there are no pooled items.
    /// </summary>
    /// <returns>The pooled item.</returns>
    T Get();

    /// <summary>
    ///   Returns the item to the pool.
    /// </summary>
    /// <param name="instance">The instance to return.</param>
    void Pool(T instance);
  }
}