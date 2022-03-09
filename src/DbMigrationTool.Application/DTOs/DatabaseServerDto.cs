namespace DbMigrationTool.Application.DTOs;

public class DatabaseServerDto
{
    public DatabaseServerDto(string name, IEnumerable<DatabaseDto> databases)
    {
        Name = name;
        Databases = databases;
    }

    public string Name { get; }
    public IEnumerable<DatabaseDto> Databases { get; }
}
