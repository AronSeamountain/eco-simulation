﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
  public class Card
  {
    private List<StatContainer> _statContainers;
    private float _offset;
    private GameObject _canvas;

    public Card(List<StatContainer> statContainers, GameObject canvas)
    {
      _statContainers = statContainers;
      _offset = 0f;
      _canvas = canvas;
    }

    public void DrawCard(GameObject canvas)
    {
      foreach (var statContainer in _statContainers)
      {
        switch (statContainer.ObjectType)
        {
          case "text":
            MakeTextObject(statContainer, canvas);
            break;
          case "slider":
            MakeSliderObject(statContainer, canvas);
            break;
        }

        _offset += 1f;
      }
    }

    private void MakeSliderObject(StatContainer statContainer, GameObject canvas)
    {
      var slider = new GameObject();
      slider.transform.SetParent(canvas.transform, false);
      slider.transform.localPosition = Vector3.zero + new Vector3(0, 2+_offset, 0);
      
      var mySlider = slider.AddComponent<Slider>();
      mySlider.value = statContainer.Value;
      mySlider.maxValue = 100;
      mySlider.minValue = 0;
      var rt = slider.GetComponent<RectTransform>();
      rt.sizeDelta = new Vector2(15, 10);
    }

    private void MakeTextObject(StatContainer statContainer, GameObject canvas)
    {
      var text = new GameObject();
      text.transform.SetParent(canvas.transform, false);
      text.transform.localPosition = Vector3.zero + new Vector3(0, 2+_offset, 0);
    
      var myText = text.AddComponent<Text>();
      myText.text = _offset.ToString();
      myText.color = statContainer.Color;
      myText.fontSize = 1;
      myText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
      myText.alignment = TextAnchor.MiddleCenter;
      var rt = text.GetComponent<RectTransform>();
      rt.sizeDelta = new Vector2(15, 10);
    }
  }
}