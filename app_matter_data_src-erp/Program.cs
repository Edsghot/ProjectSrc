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
            
            Logs.initLogs("Desarrollo.txt");
            GetCredentialsDesarrollo(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        
          
           // Application.Run(new MainValidationSunat());
           
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
            var args = argss[0].Split(',');

            if (args.Length != 6)
            {
                MessageBox.Show("Error: Se esperaban al menos 6 datos en la cadena de entrada.");
                Logs.WriteLog("ERROR", "Se esperaban al menos 6 datos en la cadena de entrada.");
                return;
            }

            var servidor = args[0];
            var user = args[1];
            var password = args[2];
            var db = args[3];
            Credentials.IdFormulario = int.Parse(args[4]);
            Credentials.DataBaseConection = "Data Source=" + servidor + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + password + ";Max Pool Size=200000;TrustServerCertificate=true";

            // Logging the values
            Logs.WriteLog("INFO", $"Servidor: {servidor}");
            Logs.WriteLog("INFO", $"User: {user}");
            Logs.WriteLog("INFO", $"Password: {password}");
            Logs.WriteLog("INFO", $"Database: {db}");
            Logs.WriteLog("INFO", $"IdFormulario: {Credentials.IdFormulario}");
            Logs.WriteLog("INFO", $"DataBaseConection: {Credentials.DataBaseConection}");
        }
        public static void GetCredentialsProduccion(string[] argss)
        {
            try
            {
                var args = argss[0].Split(',');

                if (args.Length != 6)
                {
                    MessageBox.Show("Error: Se esperaban al menos 6 datos en la cadena de entrada.");
                    Logs.WriteLog("ERROR", "Se esperaban al menos 6 datos en la cadena de entrada.");
                    return;
                }

                var servidor = HFunciones.Codificar(args[0]);
                var user = HFunciones.Codificar(args[1]);
                var password = HFunciones.Codificar(args[2]);
                var db = HFunciones.Codificar(args[3]);
                Credentials.IdFormulario = int.Parse(args[4]);
                Credentials.DataBaseConection = "Data Source=" + servidor + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + password + ";Max Pool Size=200000;TrustServerCertificate=true";

                Logs.WriteLog("INFO", "GetCredentialsProduccion executed successfully.");
            }
            catch (Exception ex)
            {
                Logs.WriteLog("ERROR", $"Exception occurred: {ex.Message}");
            }
        }



      


    }
}
