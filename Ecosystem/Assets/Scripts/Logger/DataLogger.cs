using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
  public sealed class DataLogger
  {
    //private IList<LoggableColumn> _loggableColumns;
    private const string Delimiter = ",";
    private const string Path = "Assets/Logs/TestLog.txt";

    static DataLogger()
    {
      // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    }

    private DataLogger()
    {
      // _loggableColumns = new List<LoggableColumn>();
    }

    public static DataLogger Instance { get; } = new DataLogger();

    private void Clear()
    {
      File.WriteAllText(Path, string.Empty);
    }

    /// <summary>
    ///   Clears the log file and appends the header.
    /// </summary>
    public void InitializeLogging()
    {
      Clear();
      AppendHeader();
    }

    private void AppendHeader()
    {
      var writer = File.AppendText(Path);
      writer.WriteLine("day" + Delimiter + "amount");
      writer.Close();
    }

    public void Snapshot(int day, IList<Animal> animals)
    {
      var amount = animals.Count;

      var writer = File.AppendText(Path);
      writer.WriteLine(day + Delimiter + amount);
      writer.Close();
    }

    private string CreateRow()
    {
      var sb = new StringBuilder();
      sb.Append("first column");
      sb.Append(Delimiter);
      sb.Append("second column");

      return sb.ToString();
    }
  }
}