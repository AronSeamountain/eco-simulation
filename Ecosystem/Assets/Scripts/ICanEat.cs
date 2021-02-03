/// <summary>
///   Represents something that can eat.
/// </summary>
public interface ICanEat
{
  /// <summary>
  ///   Gets the amount of saturation the entity has.
  /// </summary>
  /// <returns>The amount of saturation the entity has.</returns>
  int GetSaturation();

  /// <summary>
  ///   Adds the amount of saturation to the entity.
  /// </summary>
  /// <param name="saturation">The amount of saturation to add to the entity, must be in the range [0, Max Integer Value].</param>
  void Eat(int saturation);
}