using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Global.DtoGlobales
{
    public class ValidarComprobanteResponseDto
    {
        public string CodigoError { get; set; }
        public bool TieneError { get; set; }
        public string MensajeError { get; set; }
        public ValidarComprobanteResultadoDto Resultado { get; set; }
    }
}
