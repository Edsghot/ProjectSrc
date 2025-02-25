using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.general
{
    public class GenericErrorsDto
    {
        public  List<validationErrorDto> HeaderError { get; set; }
        public List<validationErrorDto> ErrorDetail { get; set; } 
        public bool IndError { get; set; } = false;
        public string IdRecepcion { get; set; }
    }
}
