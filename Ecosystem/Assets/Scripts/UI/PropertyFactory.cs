using System.Collections.Generic;
using Animal;
using Core;
using Foods.Plants;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public static class PropertyFactory
  {
    private static readonly Dictionary<string, GameObject> _goDictionary;
    private static readonly GameObject slider;
    private static readonly GameObject text;

    static PropertyFactory()
    {
      var gameObjects = Resources.LoadAll<GameObject>("Prefabs/UI");
      _goDictionary = new Dictionary<string, GameObject>(gameObjects.Length);

      foreach (var gameObject in gameObjects) _goDictionary.Add(gameObject.name, gameObject);

      slider = _goDictionary["Bar"];
      text = _goDictionary["Text"];
    }

    public static IList<GameObject> MakeAnimalObjects(AbstractAnimal animal)
    {
      var list = new List<GameObject>();

      var healthSlider = Object.Instantiate(slider).GetComponent<Bar>();
      healthSlider.InitializeBar(animal.GetHealthDelegate().Health, animal.GetHealthDelegate().GetMaxHealth());
      healthSlider.Color = Color.red;
      animal.GetHealthDelegate().HealthChangedListeners += healthSlider.OnValueChanged;
      list.Add(healthSlider.gameObject);

      var saturationSlider = Object.Instantiate(slider).GetComponent<Bar>();
      saturationSlider.InitializeBar(animal.GetNourishmentDelegate().Saturation,
        animal.GetNourishmentDelegate().MaxSaturation);
      saturationSlider.Color = Color.green;
      animal.GetNourishmentDelegate().SaturationChangedListeners += saturationSlider.OnValueChanged;
      list.Add(saturationSlider.gameObject);

      var hydrationSlider = Object.Instantiate(slider).GetComponent<Bar>();
      hydrationSlider.InitializeBar(animal.GetNourishmentDelegate().Hydration,
        animal.GetNourishmentDelegate().MaxHydration);
      hydrationSlider.Color = Color.cyan;
      animal.GetNourishmentDelegate().HydrationChangedListeners += hydrationSlider.OnValueChanged;
      list.Add(hydrationSlider.gameObject);


      var state = Object.Instantiate(text).GetComponent<Text>();
      state.text = "State: " + animal.GetCurrentStateEnum();
      list.Add(state.gameObject);

      var speed = Object.Instantiate(text).GetComponent<Text>();
      speed.text = "Speed: " + animal.GetSpeedModifier();
      list.Add(speed.gameObject);

      var size = Object.Instantiate(text).GetComponent<Text>();
      size.text = "Size: " + animal.GetSize();
      list.Add(size.gameObject);

      return list;
    }

    public static IList<GameObject> MakePlantObjects(Plant plant)
    {
      var list = new List<GameObject>();

      var saturation = Object.Instantiate(text).GetComponent<Text>();
      saturation.text = "Saturation: " + plant.Saturation;
      list.Add(saturation.gameObject);

      var eatable = Object.Instantiate(text).GetComponent<Text>();
      eatable.text = "Can be eaten: " + plant.CanBeEaten();
      list.Add(eatable.gameObject);

      return list;
    }

    public static IList<GameObject> MakeGlobalObjects(EntityManager entityManager)
    {
      var list = new List<GameObject>();
      
      var ecoSystemText = Object.Instantiate(text).GetComponent<Text>();
      ecoSystemText.text = "Ecosystem";
      list.Add(ecoSystemText.gameObject);
      var animalText = Object.Instantiate(text).GetComponent<Text>();
      animalText.text = "Animals: " + entityManager.Animals.Count;
      list.Add(animalText.gameObject);
      
      var plantText = Object.Instantiate(text).GetComponent<Text>();
      plantText.text = "Plants: " + entityManager.Plants.Count;
      list.Add(plantText.gameObject);
      return list;
    }
  }
}