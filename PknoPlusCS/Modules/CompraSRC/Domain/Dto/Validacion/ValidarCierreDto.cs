using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion
{
    public class ValidarCierreDto
    {
        public int numeroError { get; set; }
        public string descripcionError { get; set; }
        public bool situacion { get; set; }
        public int idUsuario { get; set; }
        public string nomUsuario { get; set; }
        public bool esExcepcion { get; set; }
        public string nomArea { get; set; }
        public int manejaTurnos { get; set; }
        public DateTime fechaCierre { get; set; }
    }
}
