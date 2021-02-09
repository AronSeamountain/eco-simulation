using System;
using System.Collections.Generic;
using System.Linq;
using AnimalStates;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   A very basic animal that searches for food.
/// </summary>
public sealed class Animal : MonoBehaviour, ICanDrink, ICanEat
{
  [SerializeField] private GoToMovement movement;
  [SerializeField] private FoodManager foodManager;
  [SerializeField] private WaterManager waterManager;
  [SerializeField] private Slider hydrationSlider;
  [SerializeField] private Slider saturationSlider;
  /// <summary>
  ///   Health bar not used at the moment.
  /// </summary>
  [SerializeField] private Slider healthBar;
  private IState _currentState;
  private FoodEaten _foodEatenListeners;
  private NourishmentDelegate _nourishmentDelegate;
  private IList<IState> _states;
  private const int Health = 100;

  public bool ShowStats { get; set; }

  public bool IsMoving => movement.HasTarget;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  public bool KnowsFoodLocation { get; private set; }

  public bool KnowsWaterLocation { get; private set; }

  /// <summary>
  ///   Returns a collection of the foods that the animal is aware of.
  /// </summary>
  public IReadOnlyCollection<Food> KnownFoods => foodManager.KnownFoodLocations;

  public Water ClosestKnownWater => waterManager.ClosestKnownWater;

  public bool IsHungry => _nourishmentDelegate.IsHungry;
  public bool IsThirsty => _nourishmentDelegate.IsThirsty;

  private void Start()
  {
    ShowStats = false;
    _nourishmentDelegate = new NourishmentDelegate();
    UpdateStats();
    _nourishmentDelegate.NourishmentChangedListeners += UpdateStats;

    // Setup states
    var pursueWaterState = new PursueWaterState();
    var pursueFoodState = new PursueFoodState();
    _states = new List<IState>
    {
      pursueWaterState,
      pursueFoodState,
      new WanderState()
    };
    _currentState = GetCorrelatingState(AnimalState.Wander);
    _currentState.Enter(this);

    // Listen to food events
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    foodManager.KnownFoodLocationsChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;
    _foodEatenListeners += foodManager.OnFoodEaten;

    //listen to water events
    waterManager.WaterUpdateListeners += OnWaterLocationChanged;
  }

  private void Update()
  {
    var newState = _currentState.Execute(this);
    if (newState != _currentState.GetStateEnum()) // Could be "cached" in the future.
    {
      _currentState.Exit(this);
      _currentState = GetCorrelatingState(newState);
      _currentState.Enter(this);
    }
  }

  public int GetHydration()
  {
    return _nourishmentDelegate.Hydration;
  }

  public void Drink(int hydration)
  {
    _nourishmentDelegate.Hydration += Mathf.Clamp(hydration, 0, int.MaxValue);
  }

  public int GetSaturation()
  {
    return _nourishmentDelegate.Saturation;
  }

  public void Eat(int saturation)
  {
    _nourishmentDelegate.Saturation += Mathf.Clamp(saturation, 0, int.MaxValue);
  }

  private void OnWaterLocationChanged(Water water)
  {
    KnowsWaterLocation = water != null;
  }

  /// <summary>
  ///   Decrease hydration and saturation over time.
  /// </summary>
  public void HydrationSaturationTicker()
  {
    _nourishmentDelegate.Tick(Time.deltaTime);
  }

  /// <summary>
  ///   Gets called when the list of known foods are changed. Sets the KnownFoodLocation to true if there is any foods in the
  ///   provided list.
  /// </summary>
  /// <param name="foods">The list of known foods.</param>
  private void OnKnownFoodLocationsChanged(IReadOnlyCollection<Food> foods)
  {
    KnowsFoodLocation = foods.Any();
  }

  /// <summary>
  ///   Eats the provided food (removes its saturation). Removes it.
  /// </summary>
  /// <param name="food">The food to eat.</param>
  public void Eat(Food food)
  {
    movement.Stop();
    Eat(food.Saturation);
    _foodEatenListeners?.Invoke(food);
    food.Consume();
  }

  /// <summary>
  ///   Moves the Animal
  /// </summary>
  /// <param name="pos">The position to go to</param>
  public void GoTo(Vector3 pos)
  {
    movement.Target = pos;
  }

  /// <summary>
  ///   Gets the state with the provided state enum.
  /// </summary>
  /// <param name="stateEnum">The state to get the state instance from.</param>
  /// <returns>The state correlating to the state enum.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If the animal has no state for the provided state enum.</exception>
  private IState GetCorrelatingState(AnimalState stateEnum)
  {
    var state = _states.First(s => s.GetStateEnum() == stateEnum);
    if (state != null) return state;

    throw new ArgumentOutOfRangeException(nameof(state), stateEnum, null);
  }

  public void StopMoving()
  {
    movement.Stop();
  }

  public void Drink(Water water)
  {
    Drink(water.Hydration);
  }

  private void UpdateStats()
  {
    saturationSlider.gameObject.SetActive(ShowStats);
    hydrationSlider.gameObject.SetActive(ShowStats);
    healthBar.gameObject.SetActive(ShowStats);
    hydrationSlider.value = GetHydration() / (float) _nourishmentDelegate.MaxHydration;
    saturationSlider.value = GetSaturation() / (float) _nourishmentDelegate.MaxSaturation;
  }

  /// <summary>
  ///   Gets invoked when the animal eats a food.
  /// </summary>
  /// <param name="food">The food that was eaten.</param>
  private delegate void FoodEaten(Food food);
}