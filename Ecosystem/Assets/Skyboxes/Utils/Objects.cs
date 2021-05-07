using System;

namespace Utils
{
  /// <summary>
  ///   Helper methods for all things objects.
  /// </summary>
  public static class Objects
  {
    /// <summary>
    ///   Checks if given object is null, throws a null reference exception if it is null.
    ///   Returns the object otherwise.
    /// </summary>
    /// <param name="obj">The object to check if null.</param>
    /// <param name="message">Message to print if it null.</param>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <exception cref="NullReferenceException">If the object is null.</exception>
    /// <returns>The object.</returns>
    public static T RequireNonNull<T>(T obj, string message = "object can not be null.")
    {
      if (obj == null)
        throw new NullReferenceException(message);

      return obj;
    }
  }
}