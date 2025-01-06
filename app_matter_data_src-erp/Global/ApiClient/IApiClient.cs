using System.Threading.Tasks;
using app_matter_data_src_erp.Global.DtoGlobales;

namespace app_matter_data_src_erp.Global.ApiClient
{
    public interface IApiClient
    {
        Task<ResponseApiGenericDto> GetApiDataAsync();
    }
}