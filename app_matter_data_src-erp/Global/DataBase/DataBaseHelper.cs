using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using app_matter_data_src_erp.Configuration.Constants;

namespace app_matter_data_src_erp.Global.DataBase
{
    public static class DataBaseHelper 
    {
        public static void ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    connection.Open();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            foreach (var param in parameters)
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error ejecutando el procedimiento almacenado " + ex.Message);
            }
        }

        public static async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error ejecutando el procedimiento almacenado " + ex.Message);
            }

            return dataTable;
        }
        public static async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error ejecutando el procedimiento almacenado '{procedureName}': {ex.Message}", "Error");
            }

            return dataTable;
        }
    }
}