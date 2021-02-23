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
      var saturation = RowFactory.CreateKeyValuePair();
      saturation.Configure("Saturation", plant.Saturation.ToString());

      var eatable = RowFactory.CreateKeyValuePair();
      eatable.Configure("Can be eaten", plant.CanBeEaten().ToString());

      return new List<MonoBehaviour> {saturation, eatable};
    }

    private static class RowFactory
    {
      private const string BarName = "Bar";
      private const string KeyValuePairTextName = "KeyValuePairText";
      private static readonly GameObject sliderPrefab;
      private static readonly GameObject KeyValuePairTextPrefab;

      static RowFactory()
      {
        var gameObjects = Resources.LoadAll<GameObject>("Prefabs/UI");
        var _goDictionary = new Dictionary<string, GameObject>(gameObjects.Length);

        foreach (var gameObject in gameObjects) _goDictionary.Add(gameObject.name, gameObject);

        sliderPrefab = _goDictionary[BarName];
        KeyValuePairTextPrefab = _goDictionary[KeyValuePairTextName];
      }

      public static Bar CreateSlider()
      {
        return Object.Instantiate(sliderPrefab).GetComponent<Bar>();
      }

      public static KeyValuePairText CreateKeyValuePair()
      {
        return Object.Instantiate(KeyValuePairTextPrefab).GetComponent<KeyValuePairText>();
      }
    }
  }
}