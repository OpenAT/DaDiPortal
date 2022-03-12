namespace DbMigrationTool.Application.DTOs;

public record ContextDto(string Name, string LatestMigration, string? LatestAppliedMigration);
