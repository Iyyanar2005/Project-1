using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connStr;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ✅ Get All Employees (SP)
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var result = await connection.QueryAsync<Employee>(
                    "spGetEmployees",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        // ✅ Get Employees By Name (SP)
        public async Task<IEnumerable<Employee>> GetEmployeesByName(string Name)
         {
            using (var connection = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@Name", Name);
                parameters.Add("@Name", string.IsNullOrEmpty(Name) ? "" : Name);
         
                var result = await connection.QueryAsync<Employee>(
                    "spGetEmployeesByDept",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
         
                return result;
            }
         }
    }
}