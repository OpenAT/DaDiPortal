namespace DbMigrationTool.Application.DTOs;

public class DatabaseDto
{
    public DatabaseDto(string name, string? latestAppliedMigration = null)
    {
        Name = name;
        LatestAppliedMigration = latestAppliedMigration;
    }

    public string Name { get; }
    public string? LatestAppliedMigration { get; set; }
}
