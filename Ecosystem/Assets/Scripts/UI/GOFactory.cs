﻿using System.Collections.Generic;
using Foods.Plants;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
  public static class GOFactory
  {
    private static Dictionary<string, GameObject> _goDictionary;
    private static GameObject slider;
    private static GameObject text;

    static GOFactory()
    {
      var gameObjects = Resources.LoadAll<GameObject>("Prefabs/UI");
      Debug.Log(gameObjects.Length);
      _goDictionary = new Dictionary<string, GameObject>(gameObjects.Length);
      
      foreach (var gameObject in gameObjects)
      {
        _goDictionary.Add(gameObject.name, gameObject);
      }

      slider = _goDictionary["Bar"];
      text = _goDictionary["Text"];
    }

    public static IList<GameObject> MakeAnimalObjects(Animal animal)
    {
      Debug.Log(_goDictionary.Count + " " + _goDictionary.ContainsKey("Overlay"));

      var list = new List<GameObject>();

      var healthSlider = Object.Instantiate(slider).GetComponent<Bar>();
      healthSlider.Value = animal._healthDelegate.Health;
      healthSlider.Color = Color.red;
      animal._healthDelegate.HealthChangedListeners += healthSlider.OnValueChanged;
      list.Add(healthSlider.gameObject);

      var saturationSlider = Object.Instantiate(slider).GetComponent<Bar>();
      saturationSlider.Color = Color.green;
      animal._nourishmentDelegate.SaturationChangedListeners += saturationSlider.OnValueChanged;
      list.Add(saturationSlider.gameObject);
      
      var hydrationSlider = Object.Instantiate(slider).GetComponent<Bar>();
      hydrationSlider.Color = Color.cyan;
      animal._nourishmentDelegate.HydrationChangedListeners += hydrationSlider.OnValueChanged;
      list.Add(hydrationSlider.gameObject);
      

      var state = Object.Instantiate(text).GetComponent<Text>();
      state.text = "State: " + animal.GetCurrentState();
      list.Add(state.gameObject);
      
      var speed = Object.Instantiate(text).GetComponent<Text>();
      speed.text = "Speed: "; //+ animal.GetSpeed();
      list.Add(speed.gameObject);
      
      var size = Object.Instantiate(text).GetComponent<Text>();
      size.text = "Size: "; // + animal.GetSize();
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
  }
}