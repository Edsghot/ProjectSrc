using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal
{
    public class SucursalDto
    {
        public string IdPuntoVenta { get; set; }
        public string NomPuntoVenta { get; set; }
        public string LocalFisico { get; set; }
        public string SucursalSRC { get; set; }
        public string NomAlmacen { get; set; }
        public string IdAlmacen { get; set; }
        public string AlmacenSrc { get; set; }
    }
}
