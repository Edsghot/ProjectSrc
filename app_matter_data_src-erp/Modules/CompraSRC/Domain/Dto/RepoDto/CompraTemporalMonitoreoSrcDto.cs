using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto
{
    public class CompraTemporalMonitoreoSrcDto
    {
        public string CodigoCompra { get; set; }
        public int Id { get; set; }
        public int? IdComputadora { get; set; }
        public string TipoDoc { get; set; }
        public string SerieCompra { get; set; }
        public string NumCompra { get; set; }
        public string RucPersona { get; set; }
        public string NomPersona { get; set; }
        public string Sucursal { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaVenc { get; set; }
        public string Moneda { get; set; }
        public decimal? TcCompra { get; set; }
        public string Condicion { get; set; }
        public string NomTransportista { get; set; }
        public string RucTransportista { get; set; }
        public string DirTransportista { get; set; }
        public string PlacaTransportista { get; set; }
        public string MarcaTransportista { get; set; }
        public string CertInscripcion { get; set; }
        public string ConfiguracionVeh { get; set; }
        public string Cubicacion { get; set; }
        public string NomChofer { get; set; }
        public string BreveteChofer { get; set; }
        public int? DestinoRC { get; set; }
        public string Obs { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Igv { get; set; }
        public decimal? Total { get; set; }
        public string NDetraccion { get; set; }
        public DateTime? FDetraccion { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public bool? PrecioIncluyeIGV { get; set; }
        public int? TipoOperacion { get; set; }
        public int? CentroCostos { get; set; }
        public string SeriePer { get; set; }
        public string NumPer { get; set; }
        public DateTime? FechaPercepcion { get; set; }
        public decimal? PerTotal { get; set; }
        public decimal? PRetencion { get; set; }
        public bool? ValidarTotales { get; set; }
        public string IdProductoExt { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public int? TipoIGV { get; set; }
        public decimal? PIGV { get; set; }
        public DateTime? FechaVencProducto { get; set; }
        public decimal? Api { get; set; }
        public decimal? Temperatura { get; set; }
        public decimal? IgvCosto { get; set; }
        public string SerieProducto { get; set; }
        public int? Estado { get; set; }
        public string NomProductoSrc { get; set; }
        public string IdRecepcionSrc { get; set; }
        public int IdPeriodo { get;set; }
        public DateTime FechaPeriodo { get; set; }

        public string Accion { get; set; } = "Editar";
        public string EstadoComprobante { get; set; }
    }

}
