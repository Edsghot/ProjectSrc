using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.bitacora
{
    public class DetalleTemporalBitacoraSrcDto
    {
        public int Idtemporal { get; set; }
        public string IdRecepcionSrc { get; set; }
        public int IdProductoSrc { get; set; }
        public string NomProductoSrc { get; set; }
        public decimal Api { get; set; }
        public decimal Temp { get; set; }
        public DateTime Fecha { get; set; }
    }
}
