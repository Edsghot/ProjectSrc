namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto
{
    public class CompraDetalleDto
    {
        public string Codigo { get; set; }
        public string Serie { get; set; }
        public bool TieneSerie { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Api { get; set; }
        public decimal Temp { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Fise { get; set; }
        public decimal Dscto { get; set; }
        public decimal Isc { get; set; }
        public int TieneIGV { get; set; }
        public decimal Igv { get; set; }
        public int Tratamiento { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}