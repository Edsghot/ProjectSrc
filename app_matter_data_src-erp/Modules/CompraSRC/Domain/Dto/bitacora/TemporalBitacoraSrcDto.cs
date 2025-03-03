using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.bitacora
{
    public class TemporalBitacoraSrcDto
    {
        public int IdTemporal { get; set; }
        public string IdRecepcionSrc { get; set; }
        public string Serie { get; set; }
        public string NumCompra { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public string Scop { get; set; }
        public int? IdPeriodo { get; set; }
        public DateTime? FechaPeriodo { get; set; }
        public decimal? FiseTotal { get; set; }
    }
}
