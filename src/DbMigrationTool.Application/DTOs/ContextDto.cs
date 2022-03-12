namespace DbMigrationTool.Application.DTOs;

public class ContextDto
{
    public ContextDto(Type contextType, string latestMigration)
    {
        ContextType = contextType;
        LatestMigration = latestMigration;
    }

    public Type ContextType { get; }
    public string LatestMigration { get; }
    public string? LatestAppliedMigration { get; set; }
}
