namespace AnimalStates
{
  /// <summary>
  ///   A state for a finite state machine.
  /// </summary>
  public interface IState
  {
    /// <summary>
    ///   Setups the state.
    /// </summary>
    /// <param name="animal">The animal of the state.</param>
    void Enter(Animal animal);

    /// <summary>
    ///   Gets called for every frame, should be used as Unitys Update() method.
    /// </summary>
    /// <param name="animal">The animal of the state.</param>
    IState Execute(Animal animal);

    /// <summary>
    ///   Exits the state and cleans up after itself.
    /// </summary>
    /// <param name="animal">The animal of the state.</param>
    void Exit(Animal animal);
  }
}