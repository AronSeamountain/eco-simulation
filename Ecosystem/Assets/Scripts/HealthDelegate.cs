using UnityEngine;

public class HealthDelegate : MonoBehaviour
{
  private Animal _animal;
  

  private void Delay5sec()
  {
  }

  public void UpdateHealth()
  {
    if (_animal._health == 0)
    {
      _animal.StopMoving();
      //Delay5sec() (don't know if needed yet)
      // Remove the animal after 5 sec
    }
    else
    {
      //update _health;
    }
  }
}