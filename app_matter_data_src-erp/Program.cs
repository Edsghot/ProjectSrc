using app_matter_data_src_erp.Configuration.Constants;
using app_matter_data_src_erp.Modules;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_matter_data_src_erp.Configuration.Enum;
using app_matter_data_src_erp.Configuration.Logs;
using app_matter_data_src_erp.Global.Helper;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Global.DataBase;

namespace app_matter_data_src_erp
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            MapperConfig.RegisterMappings();

            Logs.initLogs("Desarrollo.txt");
            GetCredentialsDesarrollo(args);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(GlobalExceptionHandler);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);




            //var resultado = repo.InsertProdCuencidencia(new InsertProdCuencidenciaDto
            //{
            //    IdProductoErp = "1",
            //    NombreProdErp = "Producto 1",
            //    NombreProdSrc = "Producto 1",
            //    RucEmpresa = "123456789"
            //}).GetAwaiter();
            //var res2 = repo.getAllSucursal().GetAwaiter().GetResult();
            //var res3 = repo.ObtenerCoincidenciasProdSrcPorRuc("123456789").GetAwaiter().GetResult();
            //Application.Run(new MainValidationSunat());


            switch (Credentials.IdFormulario)
            {
                case (int)Formularios.FormularioCompraSrc:
                    Logs.initLogs("CompraSrc.txt");
                    Application.Run(new MainComprasSrc());

                    break;
                case (int)Formularios.FormularioValidation:
                    Logs.initLogs("ValidationSunat.txt");
                    Application.Run(new MainValidationSunat());

                    break;
                default:
                    break;
            }

        }

        private static void GetCredentialsDesarrollo(string[] argss)
        {
            string argsConcatenados = string.Join(", ", argss);

            // Mostrar la cadena concatenada en un MessageBox
            MessageBox.Show($"Argumentos recibidos: {argsConcatenados}");
            var args = argss[0].Split(',');

            if (args.Length != 10)
            {
                MessageBox.Show("Error: Se esperaban al menos 10 datos en la cadena de entrada.");
                Logs.WriteLog("ERROR", "Se esperaban al menos 10 datos en la cadena de entrada.");
                return;
            }
            var servidor = args[0];
            var user = args[1];
            var password = args[2];
             var db = args[3];
            //var servidor = HFunciones.Codificar(args[0]);
            //var user = HFunciones.Codificar(args[1]);
            //var password = HFunciones.Codificar(args[2]);
            //var db = HFunciones.Codificar(args[3]);
            Credentials.Ruc = args[4].ToString();
            Credentials.IdPuntoVenta = int.Parse(args[5]);
            Credentials.IdDepartamento = int.Parse(args[6]);
            Credentials.IdTurno = int.Parse(args[7]);
            Credentials.IdComputadora = int.Parse(args[8]);
            Credentials.IdFormulario = int.Parse(args[9]);
            Credentials.DataBaseConection = "Data Source=" + servidor + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + password + ";Max Pool Size=200000;TrustServerCertificate=true";

            // Logging the values
            Logs.WriteLog("INFO", $"Servidor: {servidor}");
            Logs.WriteLog("INFO", $"User: {user}");
            Logs.WriteLog("INFO", $"Password: {password}");
            Logs.WriteLog("INFO", $"Database: {db}");
            Logs.WriteLog("INFO", $"RUC: {Credentials.Ruc}");
            Logs.WriteLog("INFO", $"IdPuntoVenta: {Credentials.IdPuntoVenta}");
            Logs.WriteLog("INFO", $"IdTurno: {Credentials.IdDepartamento}");
            Logs.WriteLog("INFO", $"IdComputadora: {Credentials.IdTurno}");
            Logs.WriteLog("INFO", $"IdComputadora: {Credentials.IdComputadora}");
            Logs.WriteLog("INFO", $"IdFormulario: {Credentials.IdFormulario}");
            Logs.WriteLog("INFO", $"DataBaseConection: {Credentials.DataBaseConection}");
        }

        public static void GetCredentialsProduccion(string[] argss)
        {
            try
            {
                var args = argss[0].Split(',');

                if (args.Length != 10)
                {
                    MessageBox.Show("Error: Se esperaban al menos 6 datos en la cadena de entrada.");
                    return;
                }

                var servidor = HFunciones.Codificar(args[0]);
                var user = HFunciones.Codificar(args[1]);
                var password = HFunciones.Codificar(args[2]);
                var db = HFunciones.Codificar(args[3]);
                Credentials.Ruc = args[4].ToString();
                Credentials.IdPuntoVenta = int.Parse(args[5]);
                Credentials.IdDepartamento = int.Parse(args[6]);
                Credentials.IdTurno = int.Parse(args[7]);
                Credentials.IdComputadora = int.Parse(args[8]);
                Credentials.IdFormulario = int.Parse(args[9]);

                Credentials.DataBaseConection = "Data Source=" + servidor + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + password + ";Max Pool Size=200000;TrustServerCertificate=true";

                Logs.WriteLog("INFO", "GetCredentialsProduccion executed successfully.");
            }
            catch (Exception ex)
            {
                Logs.WriteLog("ERROR", $"Exception occurred: {ex.Message}");
            }
        }

        static void GlobalExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        static void HandleException(Exception ex)
        {
            // Registra la excepción (puedes usar tu mecanismo de registro aquí)
            Logs.WriteLog("ERROR", $"Ocurrió una excepción: {ex.Message}");

            // Muestra un cuadro de mensaje para informar al usuario
            MessageBox.Show("Ocurrió un error inesperado. Por favor, contacta al soporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }






    }
}
