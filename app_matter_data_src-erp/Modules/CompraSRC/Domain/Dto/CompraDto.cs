using System;
using System.Collections.Generic;
using System.ComponentModel;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using Newtonsoft.Json;

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
    public string GuiaRemisionAsociada { get; set; }
    public string CorrelativoReferencia { get; set; }
    [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd")]
    public DateTime FechaEmisionReferencia { get; set; }
    public string PlacaTransportista { get; set; }
    public string LicenciaTransportista { get; set; }
    public string MarcaTransportista { get; set; }
    public string Errores { get; set; }
    //data src
    public int IdAlmacen { get; set; }
    public string FechaLlegada { get; set; }
    public string NewSucursal { get; set; }
    public string Estado { get; set; }

    // Propiedades adicionales
    public string idCliPro { get; set; }
    public string idClaseDoc { get; set; }
    public DateTime FechaDig { get; set; }
    public DateTime FechaOperativa { get; set; }
    public int idPeriodo { get; set; }
    public decimal TipoCambio { get; set; }
    public string NGuiaRemision { get; set; }
    public int idTransportista { get; set; }
    public int idPlaca { get; set; }
    public int idChofer { get; set; }
    public string IdPlantilla { get; set; }
    public string NomPlantilla { get; set; }
    public decimal SubTotal { get; set; }
    public bool Importacion { get; set; }
    public bool Automatica { get; set; }
    public string numConstanciaDep { get; set; }
    public DateTime? fecConstanciaDep { get; set; }
    public int IdTurno { get; set; }
    public bool RelGuiaCompra { get; set; }
    public bool PrecioIncluyeIGV { get; set; }
    public int tipoFechaRegCompras { get; set; }
    public DateTime? fechaEspecialRC { get; set; }
    public bool servicioIntangible { get; set; }
    public int idTipoOperacion { get; set; }
    public int idDepartamento { get; set; }
    public string nOrdenCompra { get; set; }
    public decimal detraccion { get; set; }
    public bool tieneConsignaciones { get; set; }
    public decimal fleteTotal { get; set; }
    public bool distribuir { get; set; }
    public int idProcesoAsociado { get; set; }
    public string nProcesoAsociado { get; set; }
    public int guiaRecibida { get; set; }
    public string nPercepcion { get; set; }
    public DateTime? fechaPercepcion { get; set; }
    public decimal pRetencion { get; set; }
    public string nCompraPlus { get; set; }
    public string nOrdenCompraProveedor { get; set; }
    public decimal fiseTotal { get; set; }
    public int idClasificacionBienesServicios { get; set; }
    public int idTipoFacturacionGuiaRemision { get; set; }

}

