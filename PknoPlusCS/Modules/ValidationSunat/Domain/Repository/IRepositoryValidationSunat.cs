using PknoPlusCS.Modules.ValidationSunat.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.ValidationSunat.Domain.Repository
{
    public interface IRepositoryValidationSunat
    {
        List<ListCpesDto> ListarCPESComprasValidados(int mes, int anio);
        ResultadoUpsertDto UpsertCPESComprasValidados(int idConfigRCV, int mes, int anio);
        int ActualizarEstadoSunatCPE(string numDocPlus, string estadoSunat);

    }
}
