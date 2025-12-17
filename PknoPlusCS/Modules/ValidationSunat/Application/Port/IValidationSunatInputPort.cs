using PknoPlusCS.Modules.ValidationSunat.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.ValidationSunat.Application.Port
{
    public interface IValidationSunatInputPort
    {
        Task<ValidacionResultadoDto> ValidarSunatAsync(List<ListCpesDto> listData, string receptorRuc);
        string ExportarExcel(List<ListCpesDto> listData, string receptorRuc, int mes, int anio);

    }
}
