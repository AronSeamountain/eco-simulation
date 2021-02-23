using System.Collections.Generic;
using UnityEngine;

namespace UI
{
  public static class RowFactory
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