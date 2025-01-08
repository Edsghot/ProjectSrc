using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcInputPort
    {
        Task<List<CompraDto>> ObtenerDataSrc();
        Task<CompraDto> ObtenerCompraPorCodigo(string codigoCompra);
    }
}
