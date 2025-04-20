using System.Net;   
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PknoPlusCS.Global.DtoGlobales;
using Newtonsoft.Json;
using System.Threading;
using RestSharp;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using Newtonsoft.Json.Linq;
using System.Globalization;
using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Proveedor;
using System.Linq;

namespace PknoPlusCS.Global.ApiClient
{
    public class ApiClient : IApiClient
    {

        public async Task<ResponseApiGenericDto> GetApiDataAsync()
        {
            try
            {
                var client = new RestClient("https://fn-ose-beta.azurewebsites.net");
                var request = new RestRequest("/api/erp/comprobante?ruc="+Credentials.Ruc, Method.GET);

                request.AddHeader("x_api_key", ApiKeySrc.apiKey);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    try
                    {
                        var jsonObject = JObject.Parse(response.Content);
                        var resultadoArray = jsonObject["Resultado"] as JArray;

                        if (resultadoArray != null && resultadoArray.Count > 0)
                        {
                            var comprasList = new List<CompraDto>();

                            for (int i = 0; i < resultadoArray.Count; i++)
                            {
                                var resultado = resultadoArray[i];

                                var compraDto = new CompraDto
                                {
                                    NomTipoDocumento = resultado["NomTipoDocumento"]?.ToString(),
                                    AbrevTipoDocumento = resultado["AbrevTipoDocumento"]?.ToString(),
                                    SerieCompra = resultado["SerieCompra"]?.ToString(),
                                    NumCompra = resultado["NumCompra"]?.ToString().TrimStart('0').PadLeft(8,'0'),
                                    DocumentoProveedor = resultado["DocumentoProveedor"]?.ToString(),
                                    TipoDocumento = resultado["TipoDocumento"]?.ToObject<int>() ?? 0,
                                    RazonSocial = resultado["RazonSocial"]?.ToString(),
                                    Sucursal = resultado["Sucursal"]?.ToString(),
                                    SucursalId = resultado["SucursalId"]?.ToString(),
                                    FechaEmision = DateTime.TryParseExact(resultado["FechaEmision"]?.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaEmision) ? fechaEmision : DateTime.MinValue,
                                    FechaVencimiento = DateTime.TryParseExact(resultado["FechaVencimiento"]?.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaVencimiento) ? fechaVencimiento : DateTime.MinValue,
                                    Moneda = resultado["Moneda"]?.ToString(),
                                    Condicion = resultado["Condicion"]?.ToString(),
                                    IdCondicion = resultado["IdCondicion"]?.ToObject<int>() ?? 0,
                                    Observacion = resultado["Observacion"]?.ToString(),
                                    TotalGravadas = resultado["TotalGravadas"]?.ToObject<decimal>() ?? 0,
                                    TotalExoneradas = resultado["TotalExoneradas"]?.ToObject<decimal>() ?? 0,
                                    TotalOtrosTributos = resultado["TotalOtrosTributos"]?.ToObject<decimal>() ?? 0,
                                    TotalInafecta = resultado["TotalInafecta"]?.ToObject<decimal>() ?? 0,
                                    TotalPercepcion = resultado["TotalPercepcion"]?.ToObject<decimal>() ?? 0,
                                    TotalIGV = resultado["TotalIGV"]?.ToObject<decimal>() ?? 0,
                                    TotalPagar = resultado["TotalPagar"]?.ToObject<decimal>() ?? 0,
                                    GuiaRemisionAsociada = resultado["GuiaRemisionAsociada"]?.ToString(),
                                    CorrelativoReferencia = resultado["CorrelativoReferencia"]?.ToString(),
                                    FechaEmisionReferencia = DateTime.TryParseExact(resultado["FechaEmisionReferencia"]?.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime FechaEmisionReferencia) ? FechaEmisionReferencia : DateTime.MinValue,
                                    PlacaTransportista = resultado["PlacaTransportista"]?.ToString(),
                                    LicenciaTransportista = resultado["LicenciaTransportista"]?.ToString(),
                                    MarcaTransportista = resultado["MarcaTransportista"]?.ToString(),
                                    IdRecepcion = resultado["idRecepcion"]?.ToString(),
                                    Compras = new List<CompraDetalleDto>()
                                };

                                if (resultado["Compras"] != null)
                                {
                                    var idCompras = 0;
                                    foreach (var detalle in resultado["Compras"])
                                    {
                                        var compraDetalle = new CompraDetalleDto
                                        {
                                            IdRecepcionComprobante = compraDto.IdRecepcion,
                                            IdProductoSrc = idCompras,
                                            Codigo = detalle["Codigo"]?.ToString(),
                                            Serie = detalle["Serie"]?.ToString(),
                                            TieneSerie = detalle["TieneSerie"]?.ToObject<bool>() ?? false,
                                            Cantidad = detalle["Cantidad"]?.ToObject<int>() ?? 0,
                                            Descripcion = detalle["Descripcion"]?.ToString(),
                                            Api = 0,
                                            Temp = 0,
                                            Fise = detalle["Fise"]?.ToObject<string>(),
                                            PrecioUnitarioSinIgv = detalle["PrecioUnitarioSinIgv"]?.ToObject<decimal>() ?? 0,
                                            PrecioUnitarioConIgv = detalle["PrecioUnitarioConIgv"]?.ToObject<decimal>() ?? 0,
                                            Dscto = detalle["dscto"].ToObject<decimal>(),
                                            Isc = detalle["ISC"]?.ToObject<decimal>() ?? 0,
                                            TieneIGV = detalle["TieneIGV"]?.ToObject<bool>() ?? false,
                                            Igv = detalle["IGV"]?.ToObject<decimal>() ?? 0,
                                            Tratamiento = detalle["Tratamiento"]?.ToString(),
                                            SubTotalSinIgv = detalle["SubTotalSinIgv"]?.ToObject<decimal>() ?? 0,
                                            SubTotalConIgv = detalle["SubTotalConIgv"]?.ToObject<decimal>() ?? 0,
                                            Total = detalle["Total"]?.ToObject<decimal>() ?? 0
                                        };

                                        idCompras++;

                                        compraDto.Compras.Add(compraDetalle);
                                    }
                                }

                                comprasList.Add(compraDto);
                            }

                            return new ResponseApiGenericDto
                            {
                                MensajeError = "Success",
                                TieneError = false,
                                Resultado = comprasList
                            };
                        }
                        else
                        {
                            return new ResponseApiGenericDto
                            {
                                MensajeError = "No se encontraron resultados.",
                                TieneError = true,
                            };
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        return new ResponseApiGenericDto
                        {
                            MensajeError = $"JSON Exception: {jsonEx.Message}",
                            TieneError = true,
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ResponseApiGenericDto
                        {
                            MensajeError = $"Deserialization Exception: {ex.Message}",
                            TieneError = true,
                        };
                    }
                }
                else
                {
                    return new ResponseApiGenericDto
                    {
                        MensajeError = $"HTTP Error: {response.StatusCode}",
                        TieneError = true,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseApiGenericDto
                {
                    MensajeError = $"Exception: {ex.Message}",
                    TieneError = true,
                };
            }
        }
       


        public async Task<ProveedorDto> GetValidSunat(string ruc)
        {
            try
            {
                var client = new RestClient("https://api.apis.net.pe");
                var request = new RestRequest($"/v2/sunat/ruc/full?numero={ruc}", Method.GET);

                // Añadir el token de autenticación
                request.AddHeader("Authorization", "Bearer apis-token-12636.0Asvi6Ccr2gs17peOWeh1WApXmvRB8z5");

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<ProveedorDto>(response.Content);
                    
                }
                else
                {
                    return new ProveedorDto();
                }
            }
            catch (Exception ex)
            {
                return new ProveedorDto();
            }
        }

        public async Task<ResponseApiGenericDto> PutComprobanteAsync(string idRecepcion,bool status)
        {
            try
            {
                var client = new RestClient("https://fn-ose-beta.azurewebsites.net");
                var request = new RestRequest($"/api/recepcion/comprobante?idRecepcion={idRecepcion}&status="+status+ "&ruc="+Credentials.Ruc, Method.PUT);

       
                request.AddHeader("x_api_key", ApiKeySrc.apiKey);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<ResponseApiGenericDto>(response.Content);
                    return result;
                }
                else
                {
                    return new ResponseApiGenericDto
                    {
                        TieneError = true,
                        MensajeError = "Error desconocido",
                        CodigoError = response.StatusCode.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseApiGenericDto
                {
                    TieneError = true,
                    MensajeError = ex.Message,
                    CodigoError = "500" 
                };
            }
        }


    }
}