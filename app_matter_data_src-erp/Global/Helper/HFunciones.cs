using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Constantes;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System;
using System.Text;

namespace app_matter_data_src_erp.Global.Helper
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

                if (data.EstadoProductos && data.EstadoFechaLlegada && data.EstadoSucursal && data.EstadoAlmacen && data.EstadoAsiento)
                {
                    data.Estado = StatusConstant.Listo;
                }

            }
        }
    }
}