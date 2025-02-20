using app_matter_data_src_erp.Configuration.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace app_matter_data_src_erp.Global.DataBase
{
    public static class DataBaseHelper
    {
        // Variable estática para almacenar la conexión abierta
        private static SqlConnection _connection;

        // Método para abrir la conexión al iniciar la aplicación
        public static void OpenConnection()
        {
            try
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(Credentials.DataBaseConection);
                    _connection.Open();
                    Console.WriteLine("Conexión a la base de datos establecida.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error abriendo la conexión a la base de datos: " + ex.Message);
            }
        }

        // Método para cerrar la conexión cuando la aplicación termina
        public static void CloseConnection()
        {
            try
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    Console.WriteLine("Conexión a la base de datos cerrada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cerrando la conexión a la base de datos: " + ex.Message);
            }
        }

        // Método sincrónico para ejecutar un procedimiento almacenado sin parámetros
        public static void ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var command = new SqlCommand(procedureName, _connection))
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
                using (var command = new SqlCommand(procedureName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
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
                using (var command = new SqlCommand(procedureName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
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
                using (var command = new SqlCommand(procedureName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ejecutando el procedimiento almacenado: " + ex.Message);
            }
        }
    }
}
