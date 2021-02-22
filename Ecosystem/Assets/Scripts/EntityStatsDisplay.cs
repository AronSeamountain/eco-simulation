using AnimalStates;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatsDisplay : MonoBehaviour
{
  [SerializeField] private Slider hydrationSlider;
  [SerializeField] private Slider saturationSlider;
  [SerializeField] private Slider healthBar;

  [SerializeField] private Text text;

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

  public void OnHealthChanged(int health, int maxHealth)
  {
    if (!ShowStats) return;
    healthBar.value = health / (float) maxHealth;
  }

  public void OnNourishmentChanged(NourishmentSnapshot nourishmentSnapshot)
  {
    if (!ShowStats) return;
    saturationSlider.value = nourishmentSnapshot.Saturation / nourishmentSnapshot.MaxSaturation;
    hydrationSlider.value = nourishmentSnapshot.Hydration / nourishmentSnapshot.MaxHydration;
  }

  private void ShowSliders(bool show)
  {
    saturationSlider.gameObject.SetActive(show);
    hydrationSlider.gameObject.SetActive(show);
    healthBar.gameObject.SetActive(show);
    text.gameObject.SetActive(show);
  }

  public void OnStateChanged(AnimalState state)
  {
    text.text = state.ToString();
    switch (state)
    {
      case AnimalState.Birth:
        text.color = Color.magenta;
        break;
      case AnimalState.PursueFood:
        text.color = Color.green;
        break;
      case AnimalState.PursueWater:
        text.color = Color.blue;
        break;
      case AnimalState.Dead:
        text.color = Color.red;
        break;
      default:
        text.color = Color.black;
        break;
    }
  }
}