using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.general
{
    public class GenericErrorsDto
    {
        public  List<validationErrorDto> HeaderError { get; set; }
        public List<validationErrorDto> ErrorDetail { get; set; } 
        public bool IndError { get; set; } = false;
        public string IdRecepcion { get; set; }
    }
}
