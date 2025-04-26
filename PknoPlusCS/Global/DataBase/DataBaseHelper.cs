using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Configuration.Logs;
using ExpressMapper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace PknoPlusCS.Global.DataBase
{
    public static class DataBaseHelper
    {
        // Método sincrónico para ejecutar un procedimiento almacenado sin parámetros
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
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ejecutando el procedimiento almacenado: " + ex.Message);
            }
        }

        // Método asincrónico para ejecutar un procedimiento almacenado sin parámetros
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
                MessageBox.Show($"Error ejecutando el procedimiento almacenado: " + ex.Message);
            }

            return dataTable;
        }

        // Método asincrónico para ejecutar un procedimiento almacenado con parámetros
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
                MessageBox.Show($"Error ejecutando el procedimiento almacenado '{procedureName}': {ex.Message}", "Error");
            }

            return dataTable;
        }

        public static DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    connection.Open();
                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ejecutando el procedimiento almacenado '{procedureName}': {ex.Message}", "Error");
            }

            return dataTable;
        }

        public static DataTable ExecuteStoredProcedure(string procedureName)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    connection.Open();
                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ejecutando el procedimiento almacenado '{procedureName}': {ex.Message}", "Error");
            }

            return dataTable;
        }


        // Método sincrónico para ejecutar un procedimiento almacenado con parámetros
        public static void ExecuteStoredProcedureWithParams(string procedureName, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(Credentials.DataBaseConection))
                {
                    connection.Open();
                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ejecutando el procedimiento almacenado: " + ex.Message);
            }
        }

    }
}
