using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto
{
    public static class DatosImportadosStatic
    {
        public static List<CompraTemporalMonitoreoSrcDto> Data { get; set; }
        public static List<SucursalDto> Sucursales { get; set; }
    }
}
