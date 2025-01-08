using System;
using System.Collections.Generic;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;

public class CompraDto
{
    public string NomTipoDocumento { get; set; }
    public string AbrevTipoDocumento { get; set; }
    public string SerieCompra { get; set; }
    public string NumCompra { get; set; }
    public string DocumentoProveedor { get; set; }
    public int TipoDocumento { get; set; }
    public string RazonSocial { get; set; }
    public string Sucursal { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Moneda { get; set; }
    public string Condicion { get; set; }
    public string Observacion { get; set; }
    public string Scop { get; set; }
    public decimal TotalGravadas { get; set; }
    public decimal TotalExoneradas { get; set; }
    public decimal TotalOtrosTributos { get; set; }
    public decimal TotalPercepcion { get; set; }
    public decimal TotalIGV { get; set; }
    public decimal TotalPagar { get; set; }
    public List<CompraDetalleDto> Compras { get; set; }
    public string TipoDocReferencia { get; set; }
    public string CorrelativoReferencia { get; set; }
    public DateTime FechaEmisionReferencia { get; set; }
    public string PlacaTransportista { get; set; }
    public string LicenciaTransportista { get; set; }
    public string MarcaTransportista { get; set; }
    public string Errores { get; set; }
    public string Estado { get; set; }
    //data src
    public string FechaLlegada { get; set; }
}

