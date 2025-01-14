﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto
{
    public class CoincidenciaProdSrcDto
    {

        public int IdCoincidencia { get; set; }
        public string IdProductoErp { get; set; }
        public string NombreProdErp { get; set; }
        public string NombreProdSrc { get; set; }
        public string RucEmpresa { get; set; }
        public bool Validado { get; set; }
        public DateTime FechaValidacion { get; set; }
    }
}