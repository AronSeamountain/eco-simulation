using System.Collections.Generic;
using Animal;
using Animal.AnimalStates;
using Foods.Plants;
using UnityEngine;
using UnityEngine.UI;

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
      var state = RowFactory.CreateText();
      state.text = "State: " + animal.GetCurrentStateEnum();

      // Speed
      var speed = RowFactory.CreateText();
      speed.text = "Speed: " + animal.GetSpeedModifier();

      // Size
      var size = RowFactory.CreateText();
      size.text = "Size: " + animal.GetSize();

      return new List<MonoBehaviour>() {healthBar, saturationSlider, hydrationSlider, state, speed, size};
    }

    public static IList<MonoBehaviour> Create(Plant plant)
    {
      var saturation = RowFactory.CreateText();
      saturation.text = "Saturation: " + plant.Saturation;

      var eatable = RowFactory.CreateText();
      eatable.text = "Can be eaten: " + plant.CanBeEaten();

      return new List<MonoBehaviour>() {saturation, eatable};
    }

    private static class RowFactory
    {
      private static readonly GameObject sliderPrefab;
      private static readonly GameObject textPrefab;

      static RowFactory()
      {
        var gameObjects = Resources.LoadAll<GameObject>("Prefabs/UI");
        var _goDictionary = new Dictionary<string, GameObject>(gameObjects.Length);

        foreach (var gameObject in gameObjects) _goDictionary.Add(gameObject.name, gameObject);

        sliderPrefab = _goDictionary["Bar"];
        textPrefab = _goDictionary["Text"];
      }

      public static Bar CreateSlider()
      {
        return Object.Instantiate(sliderPrefab).GetComponent<Bar>();
      }

      public static Text CreateText()
      {
        return Object.Instantiate(textPrefab).GetComponent<Text>();
      }
    }
  }
}