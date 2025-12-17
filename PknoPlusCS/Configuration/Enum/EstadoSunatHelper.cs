using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Configuration.Enum
{
    public enum EstadoSunatEnum
    {
        NoExiste = 0,
        Aceptado = 1,
        Anulado = 2,
        Autorizado = 3,
        NoAutorizado = 4
    }

    public static class EstadoSunatHelper
    {
        public static string ObtenerDescripcion(string estadoCp)
        {
            switch (estadoCp)
            {
                case "0": return "NO EXISTE";
                case "1": return "ACEPTADO";
                case "2": return "ANULADO";
                case "3": return "AUTORIZADO";
                case "4": return "NO AUTORIZADO";
                default: return "DESCONOCIDO";
            }
        }

        public static string ObtenerEstadoParaBD(string estadoCp)
        {
            switch (estadoCp)
            {
                case "0": return "NO EXISTE";
                case "1": return "ACEPTADO";
                case "2": return "ANULADO";
                case "3": return "AUTORIZADO";
                case "4": return "NO AUTORIZADO";
                default: return "SIN VALIDAR";
            }
        }

        /// <summary>
        /// Convierte el código de tipo de documento al código SUNAT
        /// Basado en la tabla TipoDocumentoFE
        /// </summary>
        public static string ObtenerCodigoTipoDocumento(string tipoComprobante)
        {
            if (string.IsNullOrEmpty(tipoComprobante))
                return "01";

            string tipo = tipoComprobante.ToUpper().Trim();

            // Si ya viene como código, retornarlo
            if (tipo == "01" || tipo == "03" || tipo == "07" || tipo == "08")
                return tipo;

            switch (tipo)
            {
                case "FACTURA":
                case "1":
                    return "01";
                case "BOLETA DE VENTA":
                case "BOLETA":
                case "2":
                    return "03";
                case "NOTA DE CRÉDITO":
                case "NOTA DE CREDITO":
                case "3":
                    return "07";
                case "NOTA DE DÉBITO":
                case "NOTA DE DEBITO":
                case "4":
                    return "08";
                case "RESUMEN DIARIO":
                case "5":
                    return "RC";
                case "COMUNICACIÓN DE BAJA":
                case "COMUNICACION DE BAJA":
                case "6":
                    return "RA";
                case "COMPROBANTE DE RETENCIÓN":
                case "COMPROBANTE DE RETENCION":
                case "7":
                    return "20";
                case "COMPROBANTE DE PERCEPCIÓN":
                case "COMPROBANTE DE PERCEPCION":
                case "8":
                    return "40";
                case "GUÍA DE REMISIÓN - REMITENTE":
                case "GUIA DE REMISION - REMITENTE":
                case "10":
                    return "09";
                case "GUÍA DE REMISIÓN - TRANSPORTISTA":
                case "GUIA DE REMISION - TRANSPORTISTA":
                case "11":
                    return "31";
                default:
                    return "01";
            }
        }
    }}
