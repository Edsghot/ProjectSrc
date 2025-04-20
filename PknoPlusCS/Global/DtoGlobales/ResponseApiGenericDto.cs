using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Global.DtoGlobales
{
    public class ResponseApiGenericDto
    {
        public List<CompraDto> Resultado { get; set; } = default;
        public string MensajeError { get; set; } = string.Empty;
        public bool TieneError { get; set; }
        public string CodigoError { get; set; }
    }
}
