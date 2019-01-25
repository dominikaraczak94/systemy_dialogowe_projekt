using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WashingMachineServiceWPF.Models;

namespace WashingMachineServiceWPF.Services
{
    public class DatabaseService
    {
        private SqlConnectionStringBuilder _connectionStringBuilder;

        public DatabaseService()
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder();
            _connectionStringBuilder.DataSource = "washingmachinedbserver.database.windows.net";
            _connectionStringBuilder.UserID = "wmadmin";
            _connectionStringBuilder.Password = "wmP@$$w0rd";
            _connectionStringBuilder.InitialCatalog = "WashingMachineDB";
        }

        public List<Order> ReadOrdersFromDatabase()
        {
            var orderDetails = new List<Order>();

            using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
            {
                string sqlOrderDetails = "SELECT TOP 10 * FROM [dbo].[order_history];";
                orderDetails = connection.Query<Order>(sqlOrderDetails).ToList();
            }

            return orderDetails;
        }

        public WashingTimeModel SelectProgramDefaultTime(string program)
        {
            WashingTimeModel time = new WashingTimeModel();
            using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
            {
                string sql = $@"SELECT * FROM [dbo].[washing_times] t
                              INNER JOIN [dbo].[washing_programs] p ON t.WashingProgramId = p.Id
                              WHERE p.WashingProgram = '{program}';";
                time = connection.QuerySingle<WashingTimeModel>(sql);
            }
            return time;
        }

        public List<WashingTemperatureModel> SelectProgramTemperatures(string program)
        {
            List<WashingTemperatureModel> temperatures = new List<WashingTemperatureModel>();
            using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
            {
                string sql = $@"SELECT * FROM [dbo].[washing_temperatures] t
                              INNER JOIN [dbo].[washing_programs] p ON t.WashingProgramId = p.Id
                              WHERE p.WashingProgram = '{program}';";
                temperatures = connection.Query<WashingTemperatureModel>(sql).ToList();
            }
            return temperatures;
        }

        public void InsertOrderToDatabase(Order order)
        {
            using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
            {
                var sql = @"insert into
                            [dbo].[order_history](WashingProgram, WashingTemperature, CalculatedWashingTime)
                            values(@WashingProgram, @WashingTemperature, @CalculatedWashingTime)";
                connection.Open();
                connection.Execute(sql, order);
            }
        }
    }
}