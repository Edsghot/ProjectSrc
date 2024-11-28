using app_matter_data_src_erp.Configuration.Constants;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_matter_data_src_erp
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GetCredentials();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());


        }

        public static void GetCredentials()
        {
            try
            {

                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\PecanoServiceNotification");

                if (key != null)
                {

                    Credentials.DataBaseConection = key.GetValue("ConnectionString1").ToString();
                    Credentials.DataBaseConection = key.GetValue("ConnectionString").ToString();
                    Credentials.IdComputadora = Int32.Parse(key.GetValue("IdComputadora").ToString());
                    Credentials.IdClient = key.GetValue("IdClient").ToString();
                    Credentials.IdUsuario = key.GetValue("IdUsuario").ToString();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error leyendo el registro: {ex.Message}");
            }

        }


    }
}
