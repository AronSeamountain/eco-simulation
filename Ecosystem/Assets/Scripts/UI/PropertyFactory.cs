using System.Collections.Generic;
using Animal;
using Foods.Plants;
using UnityEngine;

namespace UI
{
  public static class PropertyFactory
  {
    public static IList<MonoBehaviour> Create(AbstractAnimal animal)
    {
      // Health
      var healthBar = RowFactory.CreateSlider();
      healthBar.Configure(
        animal.GetHealthDelegate().Health,
        animal.GetHealthDelegate().GetMaxHealth(),
        Color.red
      );
      animal.GetHealthDelegate().HealthChangedListeners += healthBar.OnValueChanged;

      // Saturation
      var saturationSlider = RowFactory.CreateSlider();
      saturationSlider.Configure(
        animal.GetNourishmentDelegate().Saturation,
        animal.GetNourishmentDelegate().MaxSaturation,
        Color.green
      );
      animal.GetNourishmentDelegate().SaturationChangedListeners += saturationSlider.OnValueChanged;

      // Hydration
      var hydrationSlider = RowFactory.CreateSlider();
      hydrationSlider.Configure(
        animal.GetNourishmentDelegate().Hydration,
        animal.GetNourishmentDelegate().MaxHydration,
        Color.blue
      );
      animal.GetNourishmentDelegate().SaturationChangedListeners += hydrationSlider.OnValueChanged;

      // State name
      var state = RowFactory.CreateKeyValuePair();
      state.Configure("State", animal.GetCurrentStateEnum().ToString());
      animal.StateChangedListeners += state.OnValueChanged;

      // Speed
      var speed = RowFactory.CreateKeyValuePair();
      speed.Configure("Speed", animal.GetSpeedModifier().ToString());

      // Size
      var size = RowFactory.CreateKeyValuePair();
      size.Configure("Size", animal.GetSize().ToString());

      return new List<MonoBehaviour> {healthBar, saturationSlider, hydrationSlider, state, speed, size};
    }

    public static IList<MonoBehaviour> Create(Plant plant)
    {
      var state = RowFactory.CreateKeyValuePair();
      state.Configure("State", plant.GetCurrentStateEnum().ToString());
      plant.StateChangedListeners += state.OnValueChanged;

      var saturation = RowFactory.CreateKeyValuePair();
      saturation.Configure("Saturation", plant.Saturation.ToString());

      var eatable = RowFactory.CreateKeyValuePair();
      eatable.Configure("Eatable", plant.CanBeEaten().ToString());

      return new List<MonoBehaviour> {saturation, eatable, state};
    }
  }
}