using System.Collections.Generic;
using Animal;
using Foods.Plants;
using UI.Properties;
using UnityEngine;
using Utils;

namespace UI
{
  public static class PropertiesFactory
  {
    public static IList<AbstractProperty> Create(AbstractAnimal animal)
    {
      // Health
      var healthBar = PropertyFactory.CreateSlider();
      healthBar.Configure(
        animal.GetHealthDelegate().Health,
        animal.GetHealthDelegate().GetMaxHealth(),
        Color.red
      );
      animal.GetHealthDelegate().HealthChangedListeners += healthBar.OnValueChanged;
      healthBar.CleanupListeners += () => animal.GetHealthDelegate().HealthChangedListeners -= healthBar.OnValueChanged;

      // Saturation
      var saturationSlider = PropertyFactory.CreateSlider();
      saturationSlider.Configure(
        animal.GetNourishmentDelegate().Saturation,
        animal.GetNourishmentDelegate().MaxSaturation,
        Color.green
      );
      animal.GetNourishmentDelegate().SaturationChangedListeners += saturationSlider.OnValueChanged;
      saturationSlider.CleanupListeners += () =>
        animal.GetNourishmentDelegate().SaturationChangedListeners -= saturationSlider.OnValueChanged;

      // Hydration
      var hydrationSlider = PropertyFactory.CreateSlider();
      hydrationSlider.Configure(
        animal.GetNourishmentDelegate().Hydration,
        animal.GetNourishmentDelegate().MaxHydration,
        Color.blue
      );
      animal.GetNourishmentDelegate().SaturationChangedListeners += hydrationSlider.OnValueChanged;
      hydrationSlider.CleanupListeners += () =>
        animal.GetNourishmentDelegate().SaturationChangedListeners -= hydrationSlider.OnValueChanged;

      // State name
      var state = PropertyFactory.CreateKeyValuePair();
      state.Configure("State", animal.GetCurrentStateEnum().ToString());
      animal.StateChangedListeners += state.OnValueChanged;
      state.CleanupListeners += () => animal.StateChangedListeners -= state.OnValueChanged;

      // Speed
      var speed = PropertyFactory.CreateKeyValuePair();
      speed.Configure("Speed", Prettifier.Round(animal.GetSpeedModifier(), 2));

      // Size
      var size = PropertyFactory.CreateKeyValuePair();
      size.Configure("Size", Prettifier.Round(animal.GetSize(), 2));

      // Children
      var children = PropertyFactory.CreateKeyValuePair();
      children.Configure("Children", animal.Children.ToString());

      void ChildSpawnedImpl(AbstractAnimal child, AbstractAnimal parent)
      {
        children.OnValueChanged(parent.Children.ToString());
      }

      animal.ChildSpawnedListeners += ChildSpawnedImpl;
      children.CleanupListeners += () => animal.ChildSpawnedListeners -= (AbstractAnimal.ChildSpawned) ChildSpawnedImpl;

      return new List<AbstractProperty> {healthBar, saturationSlider, hydrationSlider, state, speed, size, children};
    }

    public static IList<AbstractProperty> Create(Plant plant)
    {
      var state = PropertyFactory.CreateKeyValuePair();
      state.Configure("State", plant.GetCurrentStateEnum().ToString());
      plant.StateChangedListeners += state.OnValueChanged;

      var saturation = PropertyFactory.CreateKeyValuePair();
      saturation.Configure("Saturation", plant.Saturation.ToString());

      var eatable = PropertyFactory.CreateKeyValuePair();
      eatable.Configure("Eatable", plant.CanBeEaten().ToString());

      return new List<AbstractProperty> {saturation, eatable, state};
    }
  }
}