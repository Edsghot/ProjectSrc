using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static
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
