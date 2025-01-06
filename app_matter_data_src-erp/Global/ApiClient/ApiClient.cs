using System.Net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using app_matter_data_src_erp.Global.DtoGlobales;
using Newtonsoft.Json;
using System.Threading;
using RestSharp;

namespace app_matter_data_src_erp.Global.ApiClient
{
    public class ApiClient : IApiClient
    {

        public async Task<ResponseApiGenericDto> GetApiDataAsync()
        {
            try
            {
                var client = new RestClient("https://unambarepoapi-production.up.railway.app/api");
                var request = new RestRequest("/Research/pecanoSrc", Method.GET);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ResponseApiGenericDto>(response.Content);
                    return new ResponseApiGenericDto
                    {
                        message = "Success",
                        success = true,
                        data = apiResponse.data
                    };
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
                return new ResponseApiGenericDto
                {
                    message = $"Exception",
                    success = true,
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiGenericDto
                {
                    message = $"Exception: {ex.Message}",
                    success = false,
                };
            }
        }


    }
}