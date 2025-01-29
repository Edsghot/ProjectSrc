using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcInputPort
    {
        Task<List<CompraDto>> ObtenerDataSrc();
        Task<CompraDto> ObtenerCompraPorCodigo(string codigoCompra);
        Task<MenuDto> GetMenu();
        Task<bool> InsertCompra(int mes, int anio, string numCompra);
        Task EscanearDCompra(string IdProduct, string NombreProducto);
    }
}
