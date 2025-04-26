using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos
{
    public class PermisosInterfacesDto
    {
        public int IdUsuario { get; set; }
        public int IdInterface { get; set; }
        public int IdAccion { get; set; }
        public bool Habilitado { get; set; }
    }
}
