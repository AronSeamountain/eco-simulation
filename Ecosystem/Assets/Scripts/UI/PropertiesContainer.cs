using System.Collections.Generic;
using UI.Properties;
using UnityEngine;

namespace UI
{
  public sealed class PropertiesContainer : MonoBehaviour
  {
    [SerializeField] private GameObject content;
    private IList<AbstractProperty> _properties;

    private void Awake()
    {
      _properties = new List<AbstractProperty>();
    }

    public void Populate(IEnumerable<AbstractProperty> properties)
    {
      if (properties == null) return;

      foreach (var property in properties)
      {
        property.transform.SetParent(content.transform, false);
        _properties.Add(property);
      }
    }

    public void ClearContent()
    {
      foreach (var property in _properties)
      {
        property.ExitCleanup();
        Destroy(property.gameObject);
      }

      _properties.Clear();
    }
  }
}