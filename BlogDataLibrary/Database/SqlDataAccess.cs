using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> LoadData<T, U>(
            string sql,
            U parameters,
            string connectionStringName)
        {
            using IDbConnection connection = new SqlConnection(
                _config.GetConnectionString(connectionStringName));

            var rows = await connection.QueryAsync<T>(sql, parameters);
            return rows.ToList();
        }

        public async Task SaveData<T>(
            string sql,
            T parameters,
            string connectionStringName)
        {
            using IDbConnection connection = new SqlConnection(
                _config.GetConnectionString(connectionStringName));

            await connection.ExecuteAsync(sql, parameters);
        }
    }
}
