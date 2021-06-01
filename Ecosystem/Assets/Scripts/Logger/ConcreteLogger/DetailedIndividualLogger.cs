using System;
using Animal;
using Core;
using UnityEngine;

namespace Logger.ConcreteLogger
{
  public class DetailedIndividualLogger : AbstractJsonLogger
  {

    public DetailedIndividualLogger()
    {
      FileName = "detailed.json";
      Path = "Assets/Logs/detailed.json";
    }
    public override void Snapshot(EntityManager entityManager)
    {
      var animals = entityManager.Animals;

      foreach (var animal in animals)
      {
        var snapshot = new AnimalSnapshot(animal, entityManager.Days);
        var snapshotJson = JsonUtility.ToJson(snapshot);
        _snapshots.Add(snapshotJson);
      }
    }
    
    /// <summary>
    ///   A snapshot of an animal. Does only contain relevant fields.
    /// </summary>
    [Serializable]
    private struct AnimalSnapshot
    {
      public string species;
      public float speed;
      public float fullyGrownSpeed;
      public float size;
      public int age;
      public int day;
      public float fullyGrownSize;
      public float visionPercentage;
      public float hearingPercentage;
      public string uuid;
      public string momUuid;
      public string gender;

      public AnimalSnapshot(AbstractAnimal animal, int day)
      {
        species = animal.Species.ToString();
        speed = animal.SpeedModifier;
        size = animal.SizeModifier;
        age = animal.AgeInDays;
        fullyGrownSize = animal.FullyGrownSize;
        fullyGrownSpeed = animal.FullyGrownSpeed;

        var totalAnimalBits = animal.VisionGene.Bits + animal.HearingGene.Bits;
        visionPercentage = animal.VisionGene.Bits * 100f / totalAnimalBits;
        hearingPercentage = animal.HearingGene.Bits * 100f / totalAnimalBits;
        this.day = day;
        uuid = animal.Uuid;
        momUuid = animal.MomUuid;
        gender = animal.Gender.ToString();
      }
    }
  }
}