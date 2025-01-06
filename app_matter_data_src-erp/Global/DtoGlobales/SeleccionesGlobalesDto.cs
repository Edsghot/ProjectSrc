using System.Collections.Generic;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;

public static class SeleccionesGlobalesDto
{
    public static string MesSeleccionado { get; set; }
    public static string AñoSeleccionado { get; set; }
}

public static class DatosGlobales
{
    public static List<CompraRDto> Compras { get; set; } = new List<CompraRDto>();
}
