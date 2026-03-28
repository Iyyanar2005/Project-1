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

         // ✅ Get Employee By ID (SP)
public async Task<Employee?> GetEmployeeByID(int empID)
{
    using (var connection = new SqlConnection(_connStr))
    {
        var parameters = new DynamicParameters();
        parameters.Add("@EmpID", empID);

        var result = await connection.QueryFirstOrDefaultAsync<Employee>(
            "spGetEmployeeByID",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result;
    }
}

// ✅ Add / Update Employee (SP)
public async Task<string> SaveEmployee(Employee emp)
{
    using (var connection = new SqlConnection(_connStr))
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new {
            emp.EmpID,
            emp.Name,
            emp.Salary
        });

        var parameters = new DynamicParameters();
        parameters.Add("@DetailsJSON", json);
        parameters.Add("@Output1", dbType: DbType.String, size: 500,
                       direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "spAddEmployeeJson",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@Output1");
    }
}
    }
}