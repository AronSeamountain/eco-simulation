namespace Animal
{
  /// <summary>
  ///   Represents something that can drink.
  /// </summary>
  public interface ICanDrink
  {
    /// <summary>
    ///   Gets the amount of hydration the entity has.
    /// </summary>
    /// <returns>The amount of hydration the entity has.</returns>
    float GetHydration();

    /// <summary>
    ///   Adds the amount of hydration to the entity.
    /// </summary>
    /// <param name="hydration">The amount of hydration to the entity, must be in the range [0, Max Integer Value].</param>
    void Drink(float hydration);
  }
}