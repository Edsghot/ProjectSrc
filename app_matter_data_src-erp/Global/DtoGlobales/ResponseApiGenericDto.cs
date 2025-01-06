using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Global.DtoGlobales
{
    public class ResponseApiGenericDto
    {
        public List<CompraDto> data { get; set; } = default;
        public string message { get; set; } = string.Empty;
        public bool success { get; set; }
    }
}
