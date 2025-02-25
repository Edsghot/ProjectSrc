using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static
{
    public static class  ControlStatic
    {
        public static bool CierreTotal { get; set; } = false;

        //PRIMER MODAL
        public static bool CierreDIalogvIew { get; set; } = false;
        public static bool CierreModalEditar { get; set; } = false;
        public static bool actualizarData { get; set; } = false;
    }
}
