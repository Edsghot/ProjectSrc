using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string IdProductoExt { get; set; }

        public int IdTipoAuxiliar { get; set; }
        public string NomTipoAuxiliar { get; set; }
     }
}
