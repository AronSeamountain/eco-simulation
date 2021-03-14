using System;
using System.Collections.Generic;
using System.IO;
using Animal;
using Core;
using UnityEngine;

namespace Logger.ConcreteLogger
{
  /// <summary>
  ///   Logs all individual animals.
  /// </summary>
  public sealed class DetailedIndividualLogger : ILogger
  {
    private const string Path = "Assets/Logs/animal_log.json";
    private IList<string> _snapshots;
    private bool _firstLog;

    static DetailedIndividualLogger()
    {
    }

    private DetailedIndividualLogger()
    {
      _snapshots = new List<string>();
      _firstLog = true;
    }

    public static DetailedIndividualLogger Instance { get; } = new DetailedIndividualLogger();

    public void Snapshot(EntityManager entityManager)
    {
      var animals = entityManager.Animals;

      foreach (var animal in animals)
      {
        var snapshot = new AnimalSnapshot(animal);
        var snapshotJson = JsonUtility.ToJson(snapshot);
        _snapshots.Add(snapshotJson);
      }
    }

    public void Persist()
    {
      if (_firstLog) Clear();
      var writer = File.AppendText(Path);
      if (_firstLog) writer.WriteLine("[");

      foreach (var snapshot in _snapshots) writer.WriteLine(snapshot + ",");
      writer.WriteLine("]");

      writer.Close();

      _snapshots = new List<string>();
      _firstLog = false;
    }

    public void Clear()
    {
      File.WriteAllText(Path, string.Empty);
    }

    /// <summary>
    ///   A snapshot of an animal. Does only contain relevant fields.
    /// </summary>
    [Serializable]
    private struct AnimalSnapshot
    {
      public string species;
      public float speed;
      public float size;
      public int age;

      public AnimalSnapshot(AbstractAnimal animal)
      {
        species = animal.Species.ToString();
        speed = animal.SpeedModifier;
        size = animal.SizeModifier;
        age = animal.AgeInDays;
      }
    }
  }
}