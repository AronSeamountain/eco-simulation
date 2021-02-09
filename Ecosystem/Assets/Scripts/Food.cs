﻿using DefaultNamespace;
using UnityEngine;

public sealed class Food : MonoBehaviour, IEatable
{
  [SerializeField] private int saturation;
  [SerializeField] private FoodType foodType;

  public int Saturation()
  {
    return saturation;
  }

  public void Consume()
  {
    Destroy(gameObject);
  }

  public FoodType FoodType()
  {
    return foodType;
  }
}