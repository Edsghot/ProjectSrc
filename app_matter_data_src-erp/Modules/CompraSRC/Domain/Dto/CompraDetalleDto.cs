using System;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto
{
    public class CompraDetalleDto
    {
        public string Codigo { get; set; }
        public string Serie { get; set; }
        public bool TieneSerie { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Api { get; set; }
        public string Temp { get; set; }
        public decimal fise { get; set; }
        public decimal PrecioUnitarioSinIgv { get; set; }
        public decimal PrecioUnitarioConIgv { get; set; }
        public string Fise { get; set; }
        public decimal Dscto { get; set; }
        public decimal Isc { get; set; }
        public bool TieneIGV { get; set; }
        public decimal Igv { get; set; }
        public string Tratamiento { get; set; }
        public decimal SubTotalSinIgv { get; set; }
        public decimal SubTotalConIgv { get; set; }
        public decimal Total { get; set; }
    }
}