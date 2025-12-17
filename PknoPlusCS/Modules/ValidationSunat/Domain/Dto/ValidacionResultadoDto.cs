using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.ValidationSunat.Domain.Dto
{
    public class ValidacionResultadoDto
    {
        public int TotalProcesados { get; set; }
        public int Exitosos { get; set; }
        public int Errores { get; set; }
        public string Mensaje { get; set; }
    }
}
