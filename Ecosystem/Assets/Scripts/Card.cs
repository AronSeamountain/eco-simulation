using UnityEngine;

public class Card : MonoBehaviour
{
  private Animal _animal;

  // Update is called once per frame
  private void Update()
  {
    GetAnimalStats();
  }
  private void GetAnimalStats()
  {
    int hunger = _animal.GetSaturation();
    int thirst = _animal.GetHydration();
  }
}
