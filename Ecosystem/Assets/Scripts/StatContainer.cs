using UnityEngine;

namespace DefaultNamespace
{
  public class StatContainer
  {
    public string ObjectType;
    public float Value;
    public Color Color;

    public StatContainer(string objectType, float value, Color color)
    {
      ObjectType = objectType;
      Value = value;
      Color = color;
    }
  }
}