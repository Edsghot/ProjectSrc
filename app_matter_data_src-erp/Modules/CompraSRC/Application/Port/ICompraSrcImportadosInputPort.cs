using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcImportadosInputPort
    {
        Task<List<CompraTemporalMonitoreoSrcDto>> ListarImportados(int estatus);
        Task<CompraTemporalMonitoreoSrcDto> GetAllByIdRecepcion(string idRecepcion);
        Task<string> GetIdRecepcion(string codigo, string ruc);
        Task<List<CompraTemporalMonitoreoSrcDto>> GetComprasPorIdRecepcion(string idRecepcion);
        Task InsertarDelTemporalActualizar(string idRecepcion);
        Task<DateTime> GetPeriodo(string idRecepcion);

    }
}
