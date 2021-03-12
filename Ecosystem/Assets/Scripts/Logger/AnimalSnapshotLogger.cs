using System;
using System.Collections.Generic;
using System.IO;
using Animal;
using UnityEngine;

namespace Logger
{
  public sealed class AnimalSnapshotLogger
  {
    private const string Path = "Assets/Logs/animal_log.json";
    private readonly IList<string> _snapshots;

    static AnimalSnapshotLogger()
    {
    }

    private AnimalSnapshotLogger()
    {
      _snapshots = new List<string>();
    }

    public static AnimalSnapshotLogger Instance { get; } = new AnimalSnapshotLogger();

    public void Persist()
    {
      File.WriteAllText(Path, string.Empty);

      var writer = File.AppendText(Path);

      writer.WriteLine("[");
      foreach (var snapshot in _snapshots) writer.WriteLine(snapshot + ",");
      writer.WriteLine("]");

      writer.Close();
    }

    public void Snapshot(IEnumerable<AbstractAnimal> animals)
    {
      foreach (var animal in animals)
      {
        var snapshot = new AnimalSnapshot(animal);
        var snapshotJson = JsonUtility.ToJson(snapshot);
        Debug.Log(snapshotJson);
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