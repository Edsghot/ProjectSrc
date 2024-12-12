using System.Collections.Generic;

public static class SeleccionesGlobales
{
    public static string MesSeleccionado { get; set; }
    public static string AñoSeleccionado { get; set; }
}

public static class DatosGlobales
{
    public static List<Compra> Compras { get; set; } = new List<Compra>();
}
