using System.Threading.Tasks;
using PknoPlusCS.Global.DtoGlobales;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Proveedor;

namespace PknoPlusCS.Global.ApiClient
{
    public interface IApiClient
    {
        Task<ResponseApiGenericDto> GetApiDataAsync();
        Task<ProveedorDto> GetValidSunat(string ruc);
        Task<ResponseApiGenericDto> PutComprobanteAsync(string idRecepcion, bool status);
        Task<ValidarComprobanteResponseDto> ValidarComprobanteSunatAsync(ValidarComprobanteRequestDto request);
    }
}