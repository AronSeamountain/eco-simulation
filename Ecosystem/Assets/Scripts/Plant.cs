using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IEatable
{

  private int _saturation = 0;

  public int Saturation()
  {
    return _saturation;
  }

  public void Consume()
  {
    _saturation = 0;
  }
}
