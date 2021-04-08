using Foods;

namespace Animal.Sensor.SensorActions
{
  public static class VisionActionFactory
  {
    /// <summary>
    ///   Creates an vision action that notifies the water found delegate.
    /// </summary>
    /// <param name="vision">The vision.</param>
    /// <returns>Returns true if the object is water since it is a "final" action.</returns>
    public static SensorAction CreateWaterSeenAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<Water>() is Water water)
        {
          vision.WaterFoundListeners?.Invoke(water);
          return true;
        }

        return false;
      });
    }

    public static SensorAction CreateEatableFoodFoundAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<AbstractFood>() is AbstractFood food && (food.CanBeEaten() || food.CanBeEatenSoon()))
        {
          vision.FoodFoundListeners?.Invoke(food);
          return true;
        }

        return false;
      });
    }

    public static SensorAction CreatePreyFoundAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<Herbivore>() is Herbivore animal && animal.CanBeEaten())
          vision.PreyFoundListeners?.Invoke(animal);

        return false;
      });
    }

    public static SensorAction CreateAnimalFoundAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<AbstractAnimal>() is AbstractAnimal foundAnimal)
          vision.AnimalFoundListeners?.Invoke(foundAnimal);

        return false;
      });
    }

    public static SensorAction CreateEnemySeenAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<AbstractAnimal>() is Carnivore carnivore)
          vision.EnemySeenListeners?.Invoke(carnivore);

        return false;
      });
    }
  }
}