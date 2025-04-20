using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto
{
    public class validationErrorDto
    {
        public string Detail { get; set; }
        public object Field { get; set; }
        public string Message { get; set; }
    }
}
