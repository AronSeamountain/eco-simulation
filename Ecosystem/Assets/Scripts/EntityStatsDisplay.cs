using UnityEngine;
using UnityEngine.UI;

public class EntityStatsDisplay : MonoBehaviour
{
  [SerializeField] private Animal animal;
  [SerializeField] private Slider hydrationSlider;
  [SerializeField] private Slider saturationSlider;

  /// <summary>
  ///   Health bar not used at the moment.
  /// </summary>
  [SerializeField] private Slider healthBar;

  public bool lastShowStats;

  public bool ShowStats { get; set; }

  private void Awake()
  {
    saturationSlider.gameObject.SetActive(false);
    hydrationSlider.gameObject.SetActive(false);
    healthBar.gameObject.SetActive(false);
  }

  // Update is called once per frame
  private void Update()
  {
    ShowStats = animal.ShowStats;
    UpdateStats();
    ShowSliders();
    lastShowStats = ShowStats;
  }

  private void UpdateStats()
  {
    if (!ShowStats) return;
    hydrationSlider.value = animal.GetHydration() / (float) animal._nourishmentDelegate.MaxHydration;
    saturationSlider.value = animal.GetSaturation() / (float) animal._nourishmentDelegate.MaxSaturation;
  }

  private void ShowSliders()
  {
    if (!(ShowStats ^ lastShowStats)) return;
    saturationSlider.gameObject.SetActive(ShowStats);
    hydrationSlider.gameObject.SetActive(ShowStats);
    healthBar.gameObject.SetActive(ShowStats);
  }
}