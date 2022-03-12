namespace DbMigrationTool.Application.Services;

public interface IConnectionStringProvider
{
    void SetConnection(string server, string database);
    string GetConnectionString();
}

public class ConnectionStringProvider : IConnectionStringProvider
{
    private string? _connectionString;

    public string GetConnectionString()
    {
        return _connectionString ?? string.Empty;
    }

    public void SetConnection(string server, string database)
    {
        _connectionString = $"Server={server};Database={database};integrated security=true;MultipleActiveResultSets=true;";
    }
}
