using System.Data;
using System.Data.SqlClient;

namespace DbMigrationTool.Infrastructure;

public class SqlDataServiceBase 
{
    #region fields

    protected string? _connStr;

    #endregion

    #region retrieval methods

    protected async Task<DataRow> GetRow(string query, Dictionary<string, object>? parameters = null)
    {
        var rows = await GetRows(query, parameters);
        return rows.Single();
    }

    protected async Task<IEnumerable<DataRow>> GetRows(string query, Dictionary<string, object>? parameters = null)
    {
        var table = await GetTable(query, parameters);
        return table
            .AsEnumerable()
            .ToList()
            .AsEnumerable();
    }

    protected Task<T> GetScalar<T>(string query, Dictionary<string, object>? parameters = null)
    {
        return Task.Run(() =>
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            if (parameters != null)
                foreach (var parameter in parameters)
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);

            return (T)cmd.ExecuteScalar();
        });
    }

    private Task<DataTable> GetTable(string query, Dictionary<string, object>? parameters = null)
    {
        return Task.Run(() =>
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            if (parameters != null)
                foreach (var parameter in parameters)
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);

            using var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);

            return table;
        });
    }

    #endregion

    #region execute methods

    protected async Task Execute(string statement)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = statement;

        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception exc)
        {
            throw new Exception($"Unable to execute statement:\n{statement}", exc);
        }
    }

    #endregion

    #region helpers

    protected SqlConnection GetConnection()
    {
        if (_connStr == null)
            throw new InvalidOperationException("Please call SetConnectionString first");

        var conn = new SqlConnection(_connStr);
        conn.Open();

        return conn;
    }

    #endregion
}
