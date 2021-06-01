using Core;
using UnityEngine;

namespace Logger.ConcreteLogger
{
  public class DeathCauseLogger : AbstractJsonLogger
  {
    public DeathCauseLogger()
    {
      FileName = "deathCause.json";
      Path = "Assets/Logs/deathCause.json";
    }
    
    public override void Snapshot(EntityManager entityManager)
    {
      var deadAnimals = entityManager.DeadAnimals;

      foreach (var deadAnimal in deadAnimals)
      {
        var snapshotJson = JsonUtility.ToJson(deadAnimal);
        _snapshots.Add(snapshotJson);
      }
    }
  }
}