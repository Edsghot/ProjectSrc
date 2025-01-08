using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;

public static class CompraDtoS
{
    public static string NomTipoDocumento { get; set; }
    public static string AbrevTipoDocumento { get; set; }
    public static string SerieCompra { get; set; }
    public static string NumCompra { get; set; }
    public static string DocumentoProveedor { get; set; }
    public static int TipoDocumento { get; set; }
    public static string RazonSocial { get; set; }
    public static string Sucursal { get; set; }
    public static DateTime FechaEmision { get; set; }
    public static DateTime FechaVencimiento { get; set; }
    public static string Moneda { get; set; }
    public static string Condicion { get; set; }
    public static string Observacion { get; set; }
    public static string Scop { get; set; }
    public static decimal TotalGravadas { get; set; }
    public static decimal TotalExoneradas { get; set; }
    public static decimal TotalOtrosTributos { get; set; }
    public static decimal TotalPercepcion { get; set; }
    public static decimal TotalIGV { get; set; }
    public static decimal TotalPagar { get; set; }
    public static List<CompraDetalleDto> Compras { get; set; }
    public static string TipoDocReferencia { get; set; }
    public static string CorrelativoReferencia { get; set; }
    public static DateTime FechaEmisionReferencia { get; set; }
    public static string PlacaTransportista { get; set; }
    public static string LicenciaTransportista { get; set; }
    public static string MarcaTransportista { get; set; }
    public static string Errores { get; set; }
    public static Boolean EstadoE { get; set; } = hola();
    public static bool hola()
    {
        if(Sucursal == "")
        {
            return true;
        }
        return false;
    }
    public static List< CompraDto> data { get; set; }
    public static void init(){
        var  apiClient = new ApiClient();
        data = apiClient.GetApiDataAsync().GetAwaiter().GetResult();

    }
}

