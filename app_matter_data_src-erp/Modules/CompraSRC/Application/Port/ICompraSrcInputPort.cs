using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.general;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcInputPort
    {
        Task<List<CompraDto>> ObtenerDataSrc();
        Task<string> GetIdRecepcion(string documentoProveedor, string codigo);
        Task<CompraDto> ObtenerCompraPorIdRecepcion(string IdRecepcion);
        Task<MenuDto> GetMenu();
        Task<bool> InsertCompra(int mes, int anio, string IdRecepcion);
        Task EscanearDCompra(string IdProduct, string NombreProducto);
        Task<bool> validarImportacion(string serie, string numCompra);
        //nuevos 
        GenericErrorsDto GetErrorsDetail(string idRecepcion);
        Task<bool> ProductsValidated(string idRecepcion);

    }
}
