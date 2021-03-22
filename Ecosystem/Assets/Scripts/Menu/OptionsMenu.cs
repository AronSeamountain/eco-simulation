using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Menu
{
  public class OptionsMenu : MonoBehaviour
  {
    public static string World = "LargeWorld";

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

    public void SetWorld(int number)
    {
      if (number == 0)
      {
        World = "LargeWorld";
      }
      else if (number == 1)
      {
        World = "World";
      }

      Debug.Log("World: " + World);
    }
  }

}
