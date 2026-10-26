using Microsoft.Data.Sqlite;

namespace Bc.Battleships.Tests.Common;

/// <summary>
/// A unique SQLite temp file for a single test (class instance). The path is fixed at construction.
/// On dispose, connection pools are cleared and the database and its side files are deleted.
/// </summary>
public sealed class TestDatabase : IDisposable
{
    private static readonly string[] Suffixes = ["", "-shm", "-wal"];

    public TestDatabase(string purpose = "tests")
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"battleships.{purpose}.{System.IO.Path.GetRandomFileName()}.db");
        ConnectionString = $"Data Source={Path}";
    }

    public string Path { get; }

    public string ConnectionString { get; }

    public bool Exists => File.Exists(Path);

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();

        foreach (var suffix in Suffixes)
        {
            var file = Path + suffix;
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}
