using System.Threading.Tasks;
using app_matter_data_src_erp.Global.DtoGlobales;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Proveedor;

namespace app_matter_data_src_erp.Global.ApiClient
{
    public interface IApiClient
    {
        Task<ResponseApiGenericDto> GetApiDataAsync();
        Task<ProveedorDto> GetValidSunat(string ruc);
        Task<ResponseApiGenericDto> PutComprobanteAsync(string idRecepcion);
    }
}