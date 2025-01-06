namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto
{
    public class CompraRDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Accion { get; set; }
    }
}