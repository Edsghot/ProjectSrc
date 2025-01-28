using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Proveedor
{
    public class ProveedorDto
    {
        public string RazonSocial { get; set; } = "";
        public string TipoDocumento { get; set; } = "";
        public string NumeroDocumento { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Condicion { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Ubigeo { get; set; } = "";
        public string ViaTipo { get; set; } = "";
        public string ViaNombre { get; set; } = "";
        public string ZonaCodigo { get; set; } = "";
        public string ZonaTipo { get; set; } = "";
        public string Numero { get; set; } = "";
        public string Interior { get; set; } = "";
        public string Lote { get; set; } = "";
        public string Dpto { get; set; } = "";
        public string Manzana { get; set; } = "";
        public string Kilometro { get; set; } = "";
        public string Distrito { get; set; } = "";
        public string Provincia { get; set; } = "";
        public string Departamento { get; set; } = "";
        public bool EsAgenteRetencion { get; set; } = false;
        public string Tipo { get; set; } = "";
        public string ActividadEconomica { get; set; } = "";
        public string NumeroTrabajadores { get; set; } = "0";
        public string TipoFacturacion { get; set; } = "";
        public string TipoContabilidad { get; set; } = "";
        public string ComercioExterior { get; set; } = "";
    }
}
