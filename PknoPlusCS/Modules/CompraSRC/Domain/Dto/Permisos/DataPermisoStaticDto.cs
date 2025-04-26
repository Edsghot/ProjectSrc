using System;
using System.Collections.Generic;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos
{
    public static class DataPermisoStaticDto
    {
        public static List<PermisosInterfacesDto> dataPermisos {  get; set; }

        public static bool MigrarCompras { get; set; }
        public static bool EditarDetalle { get; set; }
        public static bool EditarMigracion { get; set; }
        public static bool Escanear { get; set; }
    }
}
