namespace Animal.Sensor.SensorActions
{
  public static class HearingActionFactory
  {
    /// <summary>
    ///   Creates an action that calls notifies the animal heard delegate when it hears an alive animal.
    /// </summary>
    /// <param name="hearing">The hearing delegate.</param>
    /// <returns>False since it is not a "final" action.</returns>
    public static SensorAction CreateAnimalHeardAction(Hearing hearing)
    {
      return new SensorAction(obj =>
        {
          if (obj.GetComponent<AbstractAnimal>() is AbstractAnimal animal && hearing.NotSelf(animal) && animal.Alive)
            hearing.AnimalHeardListeners?.Invoke(animal);

          return false;
        }
      );
    }
  }
}