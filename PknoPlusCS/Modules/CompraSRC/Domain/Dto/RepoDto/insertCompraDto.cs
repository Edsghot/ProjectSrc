using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto
{
    public class insertCompraDto
    {
        public string nCompra { get; set; }
        public string idCliPro { get; set; }
        public string idClaseDoc { get; set; }
        public int idAlmacen { get; set; }
        public DateTime FechaDig { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaOperativa { get; set; }
        public DateTime FechaVenc { get; set; }
        public int idPeriodo { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
        public string Condicion { get; set; }
        public string NGuiaRemision { get; set; }
        public int idTransportista { get; set; }
        public int idPlaca { get; set; }
        public int idChofer { get; set; }
        public string IdPlantilla { get; set; }
        public string Obs { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public bool Importacion { get; set; }
        public bool Automatica { get; set; }
        public string numConstanciaDep { get; set; }
        public DateTime? fecConstanciaDep { get; set; }
        public DateTime FechaLlegada { get; set; }
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
        public decimal percepcionTotal { get; set; }
        public decimal pRetencion { get; set; }
        public string nCompraPlus { get; set; }
        public string nOrdenCompraProveedor { get; set; }
        public decimal fiseTotal { get; set; }
        public int idClasificacionBienesServicios { get; set; }
        public int idTipoFacturacionGuiaRemision { get; set; }
    }
}
