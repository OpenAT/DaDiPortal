namespace DbMigrationTool.Application.Data;

public enum ApplyMigrationResult
{
    Success,
    UpToDate,
    MigrationError,
    DataSeedToConfigStoreFailed,
    DataSeedToUserStoreFailed
}
