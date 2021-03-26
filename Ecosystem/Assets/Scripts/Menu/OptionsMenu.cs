﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    }

    public void SetWolves(string number)
    {
      EntityManager.InitialWolves = int.Parse(number);
    }

    public void SetPlants(string number)
    {
      EntityManager.InitialPlants = int.Parse(number);
    }

    public void SetHoursInRealSeconds(string number)
    {
      float.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
      EntityManager.HoursInRealSeconds = result;
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