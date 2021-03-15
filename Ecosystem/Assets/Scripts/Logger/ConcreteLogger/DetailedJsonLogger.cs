using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Animal;
using Core;
using UnityEngine;

namespace Logger.ConcreteLogger
{
  /// <summary>
  ///   Logs all individual animals.
  /// </summary>
  public sealed class DetailedJsonLogger : ILogger
  {
    private const string Path = "Assets/Logs/animal_log.json";
    private IList<string> _snapshots;
    private bool _firstLog;

    static DetailedJsonLogger()
    {
    }

    private DetailedJsonLogger()
    {
      _snapshots = new List<string>();
      _firstLog = true;
    }

    public static DetailedJsonLogger Instance { get; } = new DetailedJsonLogger();

    public void Snapshot(EntityManager entityManager)
    {
      var animals = entityManager.Animals;

      foreach (var animal in animals)
      {
        var snapshot = new AnimalSnapshot(animal, entityManager.Days);
        var snapshotJson = JsonUtility.ToJson(snapshot);
        _snapshots.Add(snapshotJson);
      }
    }

    public void Persist()
    {
      if (_firstLog)
        Clear();
      else
        RemoveClosingBracket();

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

    /**
     * Removes the ending "]" from the json object file.
     */
    private void RemoveClosingBracket()
    {
      File.WriteAllLines(Path, File.ReadLines(Path).Where(l => l != "]").ToList());
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
      public int day;

      public AnimalSnapshot(AbstractAnimal animal, int day)
      {
        species = animal.Species.ToString();
        speed = animal.SpeedModifier;
        size = animal.SizeModifier;
        age = animal.AgeInDays;
        this.day = day;
      }
    }
  }
}