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
    private static PropertyFactory RowFactory => PropertyFactory.SharedInstance;

    public static IEnumerable<AbstractProperty> Create(AbstractAnimal animal)
    {
      var properties = new List<AbstractProperty>();

      // ---------- Health ----------
      var healthBar = RowFactory.CreateSlider();
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

      properties.Add(healthBar);

      //---------- Saturation ----------
      var saturationSlider = RowFactory.CreateSlider();
      saturationSlider.Configure(
        animal.GetNourishmentDelegate().Saturation,
        animal.GetNourishmentDelegate().MaxSaturation,
        Color.green
      );
      animal.GetNourishmentDelegate().SaturationChangedListeners += saturationSlider.OnValueChanged;
      saturationSlider.CleanupListeners += () =>
        animal.GetNourishmentDelegate().SaturationChangedListeners -= saturationSlider.OnValueChanged;

      properties.Add(saturationSlider);

      // ---------- Hydration ----------
      var hydrationSlider = RowFactory.CreateSlider();
      hydrationSlider.Configure(
        animal.GetNourishmentDelegate().Hydration,
        animal.GetNourishmentDelegate().MaxHydration,
        Color.blue
      );
      animal.GetNourishmentDelegate().HydrationChangedListeners += hydrationSlider.OnValueChanged;
      hydrationSlider.CleanupListeners += () =>
        animal.GetNourishmentDelegate().HydrationChangedListeners -= hydrationSlider.OnValueChanged;

      properties.Add(hydrationSlider);

      // ---------- State name ----------
      var state = RowFactory.CreateKeyValuePair();
      state.Configure("State", animal.GetCurrentStateEnum().ToString());
      animal.StateChangedListeners += state.OnValueChanged;
      state.CleanupListeners += () => animal.StateChangedListeners -= state.OnValueChanged;

      properties.Add(state);

      // ---------- Speed ----------
      var speed = RowFactory.CreateKeyValuePair();
      speed.Configure("Speed", Prettifier.Round(animal.SpeedModifier, 2));

      void SpeedChangedImpl()
      {
        speed.OnValueChanged(Prettifier.Round(animal.SpeedModifier, 2));
      }

      animal.PropertiesChangedListeners += SpeedChangedImpl;
      speed.CleanupListeners += () => animal.PropertiesChangedListeners -= SpeedChangedImpl;

      properties.Add(speed);

      // ---------- Size ----------
      var size = RowFactory.CreateKeyValuePair();
      size.Configure("Size", Prettifier.Round(animal.SizeModifier, 2));

      void SizeChangedImpl()
      {
        speed.OnValueChanged(Prettifier.Round(animal.SpeedModifier, 2));
      }

      animal.PropertiesChangedListeners += SizeChangedImpl;
      speed.CleanupListeners += () => animal.PropertiesChangedListeners -= SizeChangedImpl;
      properties.Add(size);
      // ---------- Children ----------
      var children = RowFactory.CreateKeyValuePair();
      children.Configure("Children", animal.Children.ToString());

      void ChildSpawnedImpl(AbstractAnimal child, AbstractAnimal parent)
      {
        children.OnValueChanged(parent.Children.ToString());
      }
      properties.Add(children);

      animal.ChildSpawnedListeners += ChildSpawnedImpl;
      children.CleanupListeners += () => animal.ChildSpawnedListeners -= ChildSpawnedImpl;

      // ---------- Age ----------
      var age = RowFactory.CreateKeyValuePair();
      age.Configure("Age", $"{animal.AgeInDays} days");

      void AgeChangedImpl(int newAge)
      {
        age.OnValueChanged($"{newAge} days");
      }
      
      properties.Add(age);

      animal.AgeChangedListeners += AgeChangedImpl;
      age.CleanupListeners += () => animal.AgeChangedListeners -= AgeChangedImpl;

      return properties;
    }

    public static IEnumerable<AbstractProperty> Create(Plant plant)
    {
      var state = RowFactory.CreateKeyValuePair();
      state.Configure("State", plant.GetCurrentStateEnum().ToString());
      plant.StateChangedListeners += state.OnValueChanged;
      state.CleanupListeners += () => plant.StateChangedListeners -= state.OnValueChanged;

      var saturationBar = RowFactory.CreateKeyValuePair();
      saturationBar.Configure("Saturation", plant.Saturation.ToString());

      var age = PropertyFactory.CreateKeyValuePair();
      age.Configure("Age", ((int) plant.AgeInHours / 24).ToString());

      void SaturationChangedImpl(float saturation)
      {
        saturationBar.OnValueChanged(Prettifier.Round(saturation, 2));
      }

      plant.SaturationChangedListeners += SaturationChangedImpl;
      saturationBar.CleanupListeners += () => plant.SaturationChangedListeners -= SaturationChangedImpl;

      return new List<AbstractProperty> {saturationBar, state, age};
    }

    public static IEnumerable<AbstractProperty> Create(EntityManager entityManager)
    {
      var ecoSystemText = RowFactory.CreateKeyValuePair();
      ecoSystemText.Configure("Ecosystem", "Forest");

      // ---------- Age ----------
      var ageText = RowFactory.CreateKeyValuePair();
      ageText.Configure("Day", entityManager.Days.ToString());

      void DayChangeImpl()
      {
        ageText.OnValueChanged(entityManager.Days.ToString());
      }

      entityManager.DayTickListeners += DayChangeImpl;
      ageText.CleanupListeners += () => entityManager.DayTickListeners -= DayChangeImpl;

      // ---------- Animals (herbivores/rabbits) ----------
      var herbivoreText = RowFactory.CreateKeyValuePair();
      herbivoreText.Configure("Rabbits", entityManager.HerbivoreCount.ToString());

      void HerbivoreUpdateImpl()
      {
        herbivoreText.OnValueChanged(entityManager.HerbivoreCount.ToString());
      }

      entityManager.HourTickListeners += HerbivoreUpdateImpl;
      herbivoreText.CleanupListeners += () => entityManager.HourTickListeners -= HerbivoreUpdateImpl;

      // ---------- Animals (carnivores/wolfs) ----------
      var carnivoreText = RowFactory.CreateKeyValuePair();
      carnivoreText.Configure("Wolfs", entityManager.CarnivoreCount.ToString());

      void AnimalUpdateImpl()
      {
        carnivoreText.OnValueChanged(entityManager.CarnivoreCount.ToString());
      }

      entityManager.HourTickListeners += AnimalUpdateImpl;
      carnivoreText.CleanupListeners += () => entityManager.HourTickListeners -= AnimalUpdateImpl;

      // ---------- Plants ----------
      var plantText = RowFactory.CreateKeyValuePair();
      plantText.Configure("Plants", entityManager.Plants.Count.ToString());

      return new List<AbstractProperty> {ecoSystemText, ageText, herbivoreText, carnivoreText, plantText};
    }
  }
}