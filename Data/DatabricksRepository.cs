using System.Data.Odbc;
using Dapper;

public class DatabricksRepository
{
    private readonly string _connectionString;

    public DatabricksRepository(IConfiguration configuration)
    {
        _connectionString = configuration["Databricks:ConnectionString"];
    }

    public async Task<IEnumerable<dynamic>> GetApplicationsDataAsync()
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetAllapplications = "select * from hackathon.ai_reimaginators.recert_applications2"; // your actual table
        var result = await connection.QueryAsync(query_GetAllapplications);
        return result;
    }
    public async Task<IEnumerable<dynamic>> GetUsersDataAsync()
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetAllUsers = "select * from hackathon.ai_reimaginators.recert_users"; // your actual table
        var result = await connection.QueryAsync(query_GetAllUsers);
        return result;
    }
    public async Task<IEnumerable<dynamic>> GetUserAccessDataAsync()
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetAllUsersAcessData = "select * from hackathon.ai_reimaginators.recert_user_access"; // your actual table
        var result = await connection.QueryAsync(query_GetAllUsersAcessData);
        return result;
    }
    public async Task<IEnumerable<dynamic>> GetRecertRequestsDataAsync()
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetAllUsersAcessData = "select * from hackathon.ai_reimaginators.recert_requests"; // your actual table
        var result = await connection.QueryAsync(query_GetAllUsersAcessData);
        return result;
    }
    public async Task<IEnumerable<dynamic>> GetOwnersApplicationsDataAsync()
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetAllapplications = "select * from hackathon.ai_reimaginators.recert_applications2 where owner_id in ('994272')"; // your actual table
        var result = await connection.QueryAsync(query_GetAllapplications);
        return result;
    }
    
     public async Task<IEnumerable<dynamic>> GetUserAccessStatsAsync(string userId,string appId, string access_id)
    {
        using var connection = new OdbcConnection(_connectionString);
        await connection.OpenAsync();

        string query_GetUserAccessStats = "select * from hackathon.ai_reimaginators.recert_user_access where user_id = '"+userId+"' and application_id = '"+appId+"'"; // your actual table
        var result = await connection.QueryAsync(query_GetUserAccessStats);
        return result;
    }
}
