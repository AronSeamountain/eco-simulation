using System.Collections.Generic;
using System.IO;
using Animal;
using UnityEngine;

namespace Logger
{
  public sealed class AnimalSnapshotLogger
  {
    private const string Path = "Assets/Logs/animal_log.json";
    private IList<AnimalSnapshot> _snapshots;

    static AnimalSnapshotLogger()
    {
      // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    }

    private AnimalSnapshotLogger()
    {
      _snapshots = new List<AnimalSnapshot>();
    }

    /// <summary>
    ///   A snapshot of an animal. Does only contain relevant fields.
    /// </summary>
    private struct AnimalSnapshot
    {
      public AnimalSpecies species;

      public AnimalSnapshot(AbstractAnimal animal)
      {
        species = animal.Species;
      }
    }

    public static AnimalSnapshotLogger Instance { get; } = new AnimalSnapshotLogger();

    public void Persist()
    {
      var jsonString = JsonUtility.ToJson(_snapshots);

      var writer = File.AppendText(Path);
      writer.WriteLine(jsonString);
      writer.Close();
    }

    public void Snapshot(IEnumerable<AbstractAnimal> animals)
    {
      foreach (var animal in animals)
      {
        var snapshot = new AnimalSnapshot(animal);
        _snapshots.Add(snapshot);
      }
    }
  }
}