using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ExpressMapper;
using PknoPlusCS.Global.DataBase;
using PknoPlusCS.Modules.ValidationSunat.Domain.Dto;
using PknoPlusCS.Modules.ValidationSunat.Domain.Repository;

namespace PknoPlusCS.Modules.ValidationSunat.Infraestructure.Repository
{
    public class ValidationSunatRepository : IRepositoryValidationSunat
    {
        public List<ListCpesDto> ListarCPESComprasValidados(int mes, int anio)
        {
            var parameters = new[]
            {
                new SqlParameter("@mes", mes),
                new SqlParameter("@anio", anio)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("spListarCPESComprasValidados", parameters);

            var listaCpes = result.AsEnumerable()
                .Select<DataRow, ListCpesDto>(row => Mapper.Map<DataRow, ListCpesDto>(row))
                .ToList();

            return listaCpes;
        }
        public ResultadoUpsertDto UpsertCPESComprasValidados(int idConfigRCV, int mes, int anio)
        {
            var parameters = new[]
            {
                new SqlParameter("@idConfigRCV", idConfigRCV),
                new SqlParameter("@mes", mes),
                new SqlParameter("@anio", anio)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("spUpsertCPESComprasValidados", parameters);

            var resultado = result.AsEnumerable()
                .Select<DataRow, ResultadoUpsertDto>(row => Mapper.Map<DataRow, ResultadoUpsertDto>(row))
                .FirstOrDefault();

            return resultado ?? new ResultadoUpsertDto();
        }
        public int ActualizarEstadoSunatCPE(string numDocPlus, string estadoSunat)
        {
            var parameters = new[]
            {
                new SqlParameter("@NumDocPlus", numDocPlus),
                new SqlParameter("@EstadoSunat", estadoSunat)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("spActualizarEstadoSunatCPE", parameters);

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["FilasAfectadas"]);
            }

            return 0;
        }
    }
}