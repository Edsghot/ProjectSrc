using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Global.DtoGlobales
{
    public class ValidarComprobanteRequestDto
    {
        public string ReceptorRUC { get; set; }
        public string EmisorRUC { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroSerie { get; set; }
        public string FechaEmision { get; set; }
        public decimal ImporteTotal { get; set; }
    }
}
