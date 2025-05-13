using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Constantes;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using System;
using System.Text;
using Microsoft.Win32;
using PknoPlusCS.Configuration.Logs;
using PknoPlusCS.Configuration.Constants;
using System.Windows;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Application = System.Windows.Application;
using System.Runtime.CompilerServices;

namespace PknoPlusCS.Global.Helper
{
    public static class HFunciones
    {
        // [DllImport("user32.dll", SetLastError = true)]
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public static Encoding encoding = Encoding.GetEncoding(1252);
        public static string Codificar(string expresion)
        {
            string strOut;
            int letraIn;
            char letra;
            byte desplazamiento;
            int letraOut;
            strOut = "";
            desplazamiento = 220;
            for (int i = 0; i < expresion.Length; i++)
            {
                letra = expresion[i];
                letraIn = Asc(letra);
                letraOut = letraIn ^ desplazamiento;
                strOut += Chr(letraOut);
            }
            return strOut;
        }
        public static int Asc(char caracter)
        {
            int codepage = Encoding.Default.CodePage;


            return (int)Encoding.Default.GetBytes(caracter.ToString())[0];
        }

        public static int Asc(string caracter)
        {
            return Asc(caracter[0]);
        }

        public static char Chr(int codigo)
        {
            if (codigo > 255)
                throw new ArgumentOutOfRangeException("CharCode", codigo, "CharCode must be between 0 and 255.");
            return Encoding.Default.GetString(new[] { (byte)codigo })[0];
        }

        public static void ActualizarEstados()
        {
            foreach (var data in DataStaticDto.data)
            {

                if (data.EstadoProductos && data.EstadoFechaLlegada && data.EstadoSucursal && data.EstadoAlmacen && data.EstadoAsiento && data.Estado == StatusConstant.NoListo)
                {
                    data.Estado = StatusConstant.Listo;
                }

            }
        }
        public static void GetConnectionStringFromRegistry()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MigrarCompraSrc");

                if (key != null)
                {
                    var apiKeyValue = key.GetValue("API_KEY")?.ToString();

                    if (!string.IsNullOrEmpty(apiKeyValue))
                    {
                        ApiKeySrc.ApiKey = apiKeyValue;
                    }
                    else
                    {
                        Logs.WriteLog("ERROR", "API_KEY no encontrado en el registro.");
                    }
                }
                else
                {
                    Logs.WriteLog("ERROR", "La clave MigrarCompraSrc no existe en el registro.");
                    MostrarErrorYSalir("ERROR", "La clave MigrarCompraSrc no existe en el regedit");

                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Logs.WriteLog("ERROR", "No tienes permisos para acceder al registro: " + ex.Message);
                MostrarErrorYSalir("ERROR", "La clave migrarCompraSrc no existe en el regedit", ex.Message);
            }
            catch (Exception ex)
            {
                Logs.WriteLog("ERROR", "Error leyendo el registro de regedit: " + ex.Message);
                MostrarErrorYSalir("ERROR", "La clave migrarCompraSrc no existe en el regedit",ex.Message);
            }
        }

        public static void MostrarErrorYSalir(string titulo, string mensaje,string detalle = "", Form formulario = null)
        {
            MessageBox.Show(mensaje+ (detalle == "" ? "": ("\n \n detalle: " + detalle)), titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (formulario != null)
            {
                formulario.Close(); // Cierra el formulario si se proporcionó
            }
            else
            {
                Environment.Exit(1);// Cierra toda la aplicación si no se pasa formulario
            }
        }
    }
}