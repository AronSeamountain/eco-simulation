using System.Collections.Generic;
using Animal;
using Foods.Plants;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public static class PropertyFactory
  {
    public static IList<GameObject> Create(AbstractAnimal animal)
    {
      var list = new List<GameObject>();

      var healthBar = RowFactory.CreateSlider();
      healthBar.InitializeBar(animal.GetHealthDelegate().Health, animal.GetHealthDelegate().GetMaxHealth());
      healthBar.Color = Color.red;
      animal.GetHealthDelegate().HealthChangedListeners += healthBar.OnValueChanged;
      list.Add(healthBar.gameObject);

      var saturationSlider = RowFactory.CreateSlider();
      saturationSlider.InitializeBar(animal.GetNourishmentDelegate().Saturation,
        animal.GetNourishmentDelegate().MaxSaturation);
      saturationSlider.Color = Color.green;
      animal.GetNourishmentDelegate().SaturationChangedListeners += saturationSlider.OnValueChanged;
      list.Add(saturationSlider.gameObject);

      var hydrationSlider = RowFactory.CreateSlider();
      hydrationSlider.InitializeBar(animal.GetNourishmentDelegate().Hydration,
        animal.GetNourishmentDelegate().MaxHydration);
      hydrationSlider.Color = Color.cyan;
      animal.GetNourishmentDelegate().HydrationChangedListeners += hydrationSlider.OnValueChanged;
      list.Add(hydrationSlider.gameObject);


      var state = RowFactory.CreateText();
      state.text = "State: " + animal.GetCurrentStateEnum();
      list.Add(state.gameObject);

      var speed = RowFactory.CreateText();
      speed.text = "Speed: " + animal.GetSpeedModifier();
      list.Add(speed.gameObject);

      var size = RowFactory.CreateText();
      size.text = "Size: " + animal.GetSize();
      list.Add(size.gameObject);

      return list;
    }

    public static IList<GameObject> Create(Plant plant)
    {
      var list = new List<GameObject>();

      var saturation = RowFactory.CreateText();
      saturation.text = "Saturation: " + plant.Saturation;
      list.Add(saturation.gameObject);

      var eatable = RowFactory.CreateText();
      eatable.text = "Can be eaten: " + plant.CanBeEaten();
      list.Add(eatable.gameObject);

      return list;
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