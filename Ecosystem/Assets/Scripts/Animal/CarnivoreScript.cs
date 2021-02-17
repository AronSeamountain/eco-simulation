using System.Collections.Generic;
using System.Linq;
using Animal;
using AnimalStates;
using UnityEngine;

public sealed class CarnivoreScript : AbstractAnimal
{
  [SerializeField] private PreyManager preyManager;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  public bool KnowsPreyLocation { get; private set; }

  /// <summary>
  ///   Returns a collection of the foods that the animal is aware of.
  /// </summary>

  public IEnumerable<PreyManager.PreyMemory> KnownPrey => preyManager.KnownPreyMemories;


  /// <summary>
  ///   Gets called when the list of known foods are changed. Sets the KnownFoodLocation to true if there is any foods in the
  ///   provided list.
  /// </summary>
  /// <param name="foods">The list of known foods.</param>
  public void OnKnownPreyLocationsChanged(IReadOnlyCollection<PreyManager.PreyMemory> animal)
  {
    KnowsPreyLocation = animal.Any();
  }

  public void Forget(PreyManager.PreyMemory memory)
  {
    preyManager.Forget(memory);
  }

  protected override List<INewState<AnimalState>> GetStates()
  {
    return new List<INewState<AnimalState>>
    {
      new DeadState(this),
      new WanderState(this),
      new PursueWaterState(this),
      new BirthState(this),
      new HuntState(this)
    };
  }
}