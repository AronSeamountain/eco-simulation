/// <summary>
///   Represents something that can eat.
/// </summary>
public interface ICanEat
{
  /// <summary>
  ///   Gets the amount of nourishment the entity has.
  /// </summary>
  /// <returns>The amount of nourishment the entity has.</returns>
  int GetNourishment();

  /// <summary>
  ///   Adds the amount of nourishment to the entity.
  /// </summary>
  /// <param name="nourishment">The amount of nourishment to add to the entity, must be in the range [0, Max Integer Value].</param>
  void Eat(int nourishment);
}