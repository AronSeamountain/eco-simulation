using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
  public void SetRabbits(string number)
  {
    EntityManager.InitialRabbits = int.Parse(number);
    Debug.Log("Rabbits: " + EntityManager.InitialRabbits);
  }

  public void SetWolves(string number)
  {
    EntityManager.InitialWolves = int.Parse(number);
    Debug.Log("Wolves: " + EntityManager.InitialWolves);
  }
  
  public void SetPlants(string number)
  {
    EntityManager.InitialPlants = int.Parse(number);
      Debug.Log("Plants: " + EntityManager.InitialPlants);
  }
  public void SetHoursInRealSeconds(string number)
  {
    EntityManager.HoursInRealSeconds = float.Parse(number);
    Debug.Log("Hours in real seconds: " + EntityManager.HoursInRealSeconds);
  }

  
}
