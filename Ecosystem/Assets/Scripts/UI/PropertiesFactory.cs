using System.Collections.Generic;
using Animal;
using Core;
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
      healthBar.CleanupListeners += () =>
      {
        animal.GetHealthDelegate().HealthChangedListeners -= healthBar.OnValueChanged;
      };

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
      animal.GetNourishmentDelegate().HydrationChangedListeners += hydrationSlider.OnValueChanged;
      hydrationSlider.CleanupListeners += () =>
        animal.GetNourishmentDelegate().HydrationChangedListeners -= hydrationSlider.OnValueChanged;

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
      children.CleanupListeners += () => animal.ChildSpawnedListeners -= ChildSpawnedImpl;

      // Age
      var age = PropertyFactory.CreateKeyValuePair();
      age.Configure("Age", $"{animal.AgeInDays} days");

      void AgeChangedImpl(int newAge)
      {
        age.OnValueChanged($"{newAge} days");
      }

      animal.AgeChangedListeners += AgeChangedImpl;
      age.CleanupListeners += () => animal.AgeChangedListeners -= AgeChangedImpl;

      return new List<AbstractProperty>
        {healthBar, saturationSlider, hydrationSlider, state, age, speed, size, children};
    }

    public static IList<AbstractProperty> Create(Plant plant)
    {
      var state = PropertyFactory.CreateKeyValuePair();
      state.Configure("State", plant.GetCurrentStateEnum().ToString());
      plant.StateChangedListeners += state.OnValueChanged;
      state.CleanupListeners += () => plant.StateChangedListeners -= state.OnValueChanged;

      var saturationBar = PropertyFactory.CreateKeyValuePair();
      saturationBar.Configure("Saturation", plant.Saturation.ToString());

      void SaturationChangedImpl(float saturation)
      {
        saturationBar.OnValueChanged(Prettifier.Round(saturation, 2));
      }

      plant.SaturationChangedListeners += SaturationChangedImpl;
      saturationBar.CleanupListeners += () => plant.SaturationChangedListeners -= SaturationChangedImpl;

      return new List<AbstractProperty> {saturationBar, state};
    }

    public static IList<AbstractProperty> Create(EntityManager entityManager)
    {
      var ecoSystemText = PropertyFactory.CreateKeyValuePair();
      ecoSystemText.Configure("Ecosystem", "Forest");

      // Age
      var ageText = PropertyFactory.CreateKeyValuePair();
      ageText.Configure("Day", entityManager.Days.ToString());

      void DayChangeImpl()
      {
        ageText.OnValueChanged(entityManager.Days.ToString());
      }

      entityManager.DayTickListeners += DayChangeImpl;
      ageText.CleanupListeners += () => entityManager.DayTickListeners -= DayChangeImpl;

      // Animals (herbivores/rabbits)
      var herbivoreText = PropertyFactory.CreateKeyValuePair();
      herbivoreText.Configure("Rabbits", entityManager.HerbivoreCount.ToString());

      void HerbivoreUpdateImpl()
      {
        herbivoreText.OnValueChanged(entityManager.HerbivoreCount.ToString());
      }

      entityManager.TickListeners += HerbivoreUpdateImpl;
      herbivoreText.CleanupListeners += () => entityManager.TickListeners -= HerbivoreUpdateImpl;

      // Animals (carnivores/wolfs)
      var carnivoreText = PropertyFactory.CreateKeyValuePair();
      carnivoreText.Configure("Wolfs", entityManager.CarnivoreCount.ToString());

      void AnimalUpdateImpl()
      {
        carnivoreText.OnValueChanged(entityManager.CarnivoreCount.ToString());
      }

      entityManager.TickListeners += AnimalUpdateImpl;
      carnivoreText.CleanupListeners += () => entityManager.TickListeners -= AnimalUpdateImpl;

      // Plants
      var plantText = PropertyFactory.CreateKeyValuePair();
      plantText.Configure("Plants", entityManager.Plants.Count.ToString());

      return new List<AbstractProperty> {ecoSystemText, ageText, herbivoreText, carnivoreText, plantText};
    }
  }
}