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
  public abstract class AbstractJsonLogger : ILogger
  {
    protected string FileName;
    protected string Path;
    private bool _firstLog;
    protected IList<string> _snapshots;

    protected AbstractJsonLogger()
    {
      _snapshots = new List<string>();
      _firstLog = true;
    }

    public abstract void Snapshot(EntityManager entityManager);

    public void Persist()
    {
      if (_firstLog)
        Clear();
      else
        RemoveClosingBracket();

      var writer = File.AppendText(Path);
      if (_firstLog) writer.WriteLine("[");

      foreach (var snapshot in _snapshots)
        if (_firstLog)
        {
          _firstLog = false;
          writer.WriteLine(snapshot);
        }
        else
        {
          writer.WriteLine("," + snapshot);
        }

      writer.WriteLine("]");
      writer.Close();

      _snapshots = new List<string>();
      _firstLog = false;
    }

    public void Clear()
    {
      File.Create(Path).Dispose();
    }

    public void Reset(int days)
    {
      if (File.Exists(Path))
      {
        var time = DateTime.Now;
        var newDirName = time.Month + "-" + time.Day + "_" + time.Hour + time.Minute + "_" + days + "Days";
        if (!Directory.Exists(newDirName))
          Directory.CreateDirectory(newDirName);
        MoveTo(newDirName);
      }
      Clear();
    }

    public void MoveTo(string newDirName)
    {
      var target = newDirName + "/" + FileName;
      if (!File.Exists(target))
        File.Move(Path, target);
    }

    /**
     * Removes the ending "]" from the json object file.
     */
    private void RemoveClosingBracket()
    {
      File.WriteAllLines(Path, File.ReadLines(Path).Where(l => l != "]").ToList());
    }
  }
}