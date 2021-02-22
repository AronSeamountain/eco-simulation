using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
  public class CardFactory : MonoBehaviour
  {
    [SerializeField] private GameObject content;

    public void Populate(IEnumerable<GameObject> gameObjects)
    {
      if (gameObjects == null) return;
      foreach (var gameO in gameObjects) gameO.transform.SetParent(content.transform, false);
    }

    public void ClearContent()
    {
      foreach (Transform child in content.transform) Destroy(child.gameObject);
    }
  }
}