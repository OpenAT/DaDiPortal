namespace DbMigrationTool.Application.Services;

public interface IDataSeeder
{
    Task<bool> SeedDataToConfigurationStore();
    Task<bool> SeedDataToUserStore();
}
