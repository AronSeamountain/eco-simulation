using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Properties
{
  public sealed class PropertyFactory : MonoBehaviour
  {
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject KeyValuePairTextPrefab;
    public static PropertyFactory SharedInstance;

    private void Awake()
    {
      SharedInstance = this;
    }

    public Bar CreateSlider()
    {
      return Instantiate(barPrefab).GetComponent<Bar>();
    }

    public KeyValuePairText CreateKeyValuePair()
    {
      return Instantiate(KeyValuePairTextPrefab).GetComponent<KeyValuePairText>();
    }
  }
}