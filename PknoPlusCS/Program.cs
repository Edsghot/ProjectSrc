using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Modules;
using System;
using System.Windows.Forms;
using PknoPlusCS.Configuration.Enum;
using PknoPlusCS.Configuration.Logs;
using PknoPlusCS.Global.Helper;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos;

namespace PknoPlusCS
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static  void Main(string[] args)
        {

            try
            {
                MapperConfig.RegisterMappings();

                Logs.initLogs("Desarrollo.txt");
                GetCredentialsProduccion(args);

                Application.ThreadException += GlobalExceptionHandler;
                AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                switch (Credentials.IdFormulario)
                {
                    case (int)Formularios.FormularioCompraSrc:
                        Logs.initLogs("CompraSrc.txt");

                        ICompraSrcRepository repository = new CompraSrcRepository();

                        HFunciones.GetConnectionStringFromRegistry(Credentials.Ruc);
                        var dataRepo = repository.GetConfiguracionInicial();

                        DataPermisoStaticDto.dataPermisos = repository.sp_ObtenerPermisosPorInterfaceYUsuarioSrc(Credentials.IdUsuario);

                        foreach (var permiso in DataPermisoStaticDto.dataPermisos)
                        {
                            switch (permiso.IdAccion)
                            {
                                case 235:
                                    DataPermisoStaticDto.MigrarCompras = permiso.Habilitado;
                                    break;
                                case 237:
                                    DataPermisoStaticDto.EditarDetalle = permiso.Habilitado;
                                    break;
                                case 238:
                                    DataPermisoStaticDto.Escanear = permiso.Habilitado;
                                    break;
                                case 236:
                                    DataPermisoStaticDto.EditarMigracion = permiso.Habilitado;
                                    break;
                            }
                        }

                        if (dataRepo.reiniciar == 2)
                        {
                            repository.UpdateConfiguracionInicial(0);
                            Application.Exit();
                            return;
                        }

                        if (dataRepo.reiniciar == 0)
                        {
                            repository.UpdateConfiguracionInicial(2);
                        }

                        Logs.WriteLog("ERROR", "Escogio el CompraSrc");
                        Application.Run(new MainComprasSrc());
                        break;
                    case (int)Formularios.FormularioValidation:
                        Logs.initLogs("ValidationSunat.txt");

                        HFunciones.GetConnectionStringFromRegistryValidationSunat(Credentials.Ruc);
                        Application.Run(new MainValidationSunat());
                        break;
                }
            }
            catch (Exception ex)
            {

                Logs.WriteLog("ERROR", "es: ."+ ex.Message + ex);
                MessageBox.Show(@"Ocurrió un error inesperado. Por favor revisa el log para más detalles.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void GetCredentialsDesarrollo(string[] argss)
        {
            string argsConcatenados = string.Join(", ", argss);

            // Mostrar la cadena concatenada en un MessageBox
           // MessageBox.Show($@"Argumentos recibidos: {argsConcatenados}");
            var args = argss[0].Split(',');

            if (args.Length != 11)
            {
                MessageBox.Show(@"Error: Se esperaban al menos 10 datos en la cadena de entrada.");
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

            Credentials.Ruc = args[4];
            Credentials.IdPuntoVenta = int.Parse(args[5]);
            Credentials.IdDepartamento = int.Parse(args[6]);
            Credentials.IdTurno = int.Parse(args[7]);
            Credentials.IdComputadora = int.Parse(args[8]);
            Credentials.IdUsuario = int.Parse(args[9]);
            Credentials.IdFormulario = int.Parse(args[10]);


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

                if (args.Length != 11)
                {
                    MessageBox.Show(@"Error: Se esperaban al menos 10 datos en la cadena de entrada.");
                    Logs.WriteLog("ERROR", "Se esperaban al menos 10 datos en la cadena de entrada.");
                    return;
                }
                //var servidor = args[0];
                //var user = args[1];
                //var password = args[2];
                //var db = args[3];
                var servidor = HFunciones.Codificar(args[0]);
                var user = HFunciones.Codificar(args[1]);
                var password = HFunciones.Codificar(args[2]);
                var db = HFunciones.Codificar(args[3]);

                Credentials.Ruc = args[4];
                Credentials.IdPuntoVenta = int.Parse(args[5]);
                Credentials.IdDepartamento = int.Parse(args[6]);
                Credentials.IdTurno = int.Parse(args[7]);

                Credentials.IdComputadora = int.Parse(args[8]);
                Credentials.IdUsuario = int.Parse(args[9]);
                Credentials.IdFormulario = int.Parse(args[10]);

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
            MessageBox.Show(@"Ocurrió un error inesperado. Por favor, contacta al soporte.

"+ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
