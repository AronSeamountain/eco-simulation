namespace Foods
{
  public interface IPlantState
  {
    /// <summary>
    ///   Gets the plant state enum.
    /// </summary>
    /// <returns>The plant state enum.</returns>
    PlantState GetStateEnum();

    /// <summary>
    ///   Setups the state.
    /// </summary>
    /// <param name="Plant">The Plant of the state.</param>
    void Enter(Plant plant);

    /// <summary>
    ///   Gets called for every frame, should be used as Unitys Update() method.
    /// </summary>
    /// <param name="Plant">The Plant of the state.</param>
    PlantState Execute(Plant plant);

    /// <summary>
    ///   Exits the state and cleans up after itself.
    /// </summary>
    /// <param name="Plant">The Plant of the state.</param>
    void Exit(Plant plant);
  }
}