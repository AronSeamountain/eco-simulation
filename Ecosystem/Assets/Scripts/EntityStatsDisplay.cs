using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EntityStatsDisplay : MonoBehaviour
{
  [SerializeField] private Slider hydrationSlider;
  [SerializeField] private Slider saturationSlider;

  /// <summary>
  ///   Health bar not used at the moment.
  /// </summary>
  [SerializeField] private Slider healthBar;

  private bool _showStats;

  public bool ShowStats
  {
    get => _showStats;
    set
    {
      _showStats = value;
      ShowSliders(ShowStats);
    } 
  }

  public void OnNourishmentChanged(NourishmentSnapshot nourishmentSnapshot) 
  {
    if (!ShowStats) return;
    saturationSlider.value = nourishmentSnapshot.Saturation / (float) nourishmentSnapshot.MaxSaturation;
    hydrationSlider.value = nourishmentSnapshot.Hydration / (float) nourishmentSnapshot.MaxHydration;
  }

  private void ShowSliders(bool show)
  {
    saturationSlider.gameObject.SetActive(show);
    hydrationSlider.gameObject.SetActive(show);
    healthBar.gameObject.SetActive(show);
  }
}