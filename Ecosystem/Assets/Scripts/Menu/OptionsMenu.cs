using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
  public class OptionsMenu : MonoBehaviour
  {
    public static string World = "LargeWorld";

    public void SetRabbits(string number)
    {
      EntityManager.InitialRabbits = number == "" ? 0 : int.Parse(number);
    }

    public void SetWolves(string number)
    {
      EntityManager.InitialWolves = number == "" ? 0 : int.Parse(number);
    }

    public void SetPlants(string number)
    {
      EntityManager.InitialPlants = number == "" ? 0 : int.Parse(number);
    }

    public void SetHoursInRealSeconds(string number)
    {
      float.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
      EntityManager.HoursInRealSeconds = result;
    }

    public void SetPerformanceMode(Toggle toggle)
    {
      EntityManager.PerformanceModeMenuOverride = toggle.isOn;
    }

    public void SetOverlappableAnimals(Toggle toggle)
    {
      //!toggle because !collisions = overlap
      EntityManager.OverlappableAnimalsMenuOverride = !toggle.isOn;
    }
    
    public void SetLogger(Toggle toggle)
    {
      EntityManager.LogMenuOverride = toggle.isOn;
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
    }
  }
}