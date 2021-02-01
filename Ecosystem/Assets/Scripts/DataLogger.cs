using System.IO;

public sealed class DataLogger
{
  private static readonly string Delimiter = ",";
  
  static DataLogger()
  {
    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
  }

  private DataLogger()
  {
  }

  public static DataLogger Instance { get; } = new DataLogger();

  public void Do()
  {
    const string path = "Assets/test.txt";

    // Clear file
    File.WriteAllText(path, string.Empty);

    // Append
    var writer = File.AppendText(path);
    writer.WriteLine("lmao");
    writer.Close();
  }
}