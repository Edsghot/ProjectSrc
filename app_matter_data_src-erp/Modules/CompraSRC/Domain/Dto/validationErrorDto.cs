﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto
{
    public class validationErrorDto
    {
        public string Field { get; set; }
        public string Message { get; set; }
        public bool Importado { get; set; } = false;
    }
}
