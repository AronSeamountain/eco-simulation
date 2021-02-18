using UnityEngine;

namespace DefaultNamespace
{
  public class StatContainer
  {
    public string ObjectType;
    public string Text;
    public float Value;
    public Color Color;

    public StatContainer(string objectType, string text, float value, Color color)
    {
      ObjectType = objectType;
      Text = text;
      Value = value;
      Color = color;
    }
  }
}