using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Global.DtoGlobales
{
    public class ResponseApiGenericDto
    {
        public List<CompraDto> Resultado { get; set; } = default;
        public string MensajeError { get; set; } = string.Empty;
        public bool TieneError { get; set; }
        public string CodigoError { get; set; }
    }
}
