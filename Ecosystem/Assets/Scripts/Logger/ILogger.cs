using Core;

namespace Logger
{
  public interface ILogger
  {
    /// <summary>
    ///   Logs data. Does not persist data.
    /// </summary>
    /// <param name="entityManager">The entity manager.</param>
    void Snapshot(EntityManager entityManager);

    /// <summary>
    ///   Persists the data. Saves all unsaved data, removes the newly saved data from the memory.
    /// </summary>
    void Persist();

    /// <summary>
    ///   Clears the log file.
    /// </summary>
    void Clear();

    /// <summary>
    /// reset the files. 
    /// </summary>
    /// <param name="days"></param>
    void Reset(int days);

    /// <summary>
    /// moves the data to a folder
    /// </summary>
    /// <param name="newDirName"></param>
    void MoveTo(string newDirName);
  }
}