using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PknoPlusCS.Configuration.Logs
{
      public static class Logs
    {
        private static string logFilePath;

        /// <summary>
        /// Constructor que inicializa la ruta del archivo de log.
        /// </summary>
        /// <param name="logFilePath">Ruta donde se guardará el archivo de log.</param>
        ///
        /// 
        public static void initLogs(string nameFile = "LogPknoPlus2.txt")
        {
            // Define la ruta base de forma fija
            var baseDirectory = @"C:\Aplicaciones\Integraciones\logs";

            // Combina la ruta base con el nombre del archivo
            logFilePath = Path.Combine(baseDirectory, nameFile);

            // Asegura que el archivo y el directorio existan
            EnsureLogFileExists();
        }


        /// <summary>
        /// Verifica si la carpeta y el archivo de log existen; si no, los crea.
        /// </summary>
        private static void EnsureLogFileExists()
        {
            var directoryPath = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            if (!File.Exists(logFilePath))
                using (File.Create(logFilePath))
                {
                }
        }

        /// <summary>
        /// Registra un mensaje en el archivo de log con el formato especificado.
        /// </summary>
        /// <param name="level">El nivel del log (INFO, WARNING, NOTIFICATION).</param>
        /// <param name="message">El mensaje a registrar.</param>
        public static void WriteLog(string level, string message, [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var formattedMessage = $"{timestamp} {level}: {message}";

            // Información adicional
            var additionalInfo = $"...................................[File: {Path.GetFileName(filePath)}] [Method: {memberName}] [Line: {lineNumber}]";

            // Concatenar toda la información
            var logMessage = $"{formattedMessage} {additionalInfo}";

            try
            {
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // Opcional: manejar errores al escribir en el log
                Console.WriteLine($"Error al escribir en el log: {ex.Message}");
            }
        }
    }
}