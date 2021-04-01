namespace Core
{
  /// <summary>
  ///   Represents something that can be "dumbed" down to boost performance.
  /// </summary>
  public interface IBoostable
  {
    /// <summary>
    ///   Lowers the quality of something, boosting the performance.
    /// </summary>
    void Boost();
  }
}