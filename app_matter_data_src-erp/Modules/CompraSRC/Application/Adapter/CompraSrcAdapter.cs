using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System.Linq;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter
{
    public class CompraSrcAdapter : ICompraSrcInputPort
    {
        private readonly IApiClient apiClient;

        public CompraSrcAdapter()
        {
            this.apiClient = new ApiClient();
        }

        public async Task<List<CompraDto>> ObtenerDataSrc()
        {
            var response = await apiClient.GetApiDataAsync();

            if (response == null || response.data == null)
            {
                return new List<CompraDto>();
            }

            return response.data;
        }

        public async Task<CompraDto> ObtenerCompraPorCodigo(string codigoCompra)
        {
            var response = await apiClient.GetApiDataAsync();

            if (response == null || response.data == null)
            {
                return null;
            }

            var compra = response.data.FirstOrDefault(c => c.NumCompra == codigoCompra);

            if (compra == null)
            {
                return null; 
            }

            return compra; 
        }

        public List<validationErrorDto> ValidateColumn(int columnIndex, string columnValue)
        {
            var errors = new List<validationErrorDto>();

            if (columnIndex == 1)
            {
                if (columnValue != null && columnValue.Length != 10)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Column1",
                        Message = "El valor de la columna 1 debe tener exactamente 10 caracteres."
                    });
                }
            }
            else if (columnIndex == 2)
            {
                if (string.IsNullOrEmpty(columnValue) || !columnValue.All(char.IsLetter))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Column2",
                        Message = "El valor de la columna 2 debe ser un texto."
                    });
                }
            }
            else if (columnIndex == 3)
            {
                if (columnValue.Length < 5 || columnValue.Length > 150)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Column3",
                        Message = "El valor de la columna 3 debe tener entre 5 y 150 caracteres."
                    });
                }
            }
            else if (columnIndex == 4)
            {
                if (!columnValue.StartsWith("5") || !columnValue.EndsWith("JH"))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Column4",
                        Message = "El valor de la columna 4 debe comenzar con '5' y terminar con 'JH'."
                    });
                }
            }
            return errors;
        }

        public List<validationErrorDto> ValidateCompra(CompraDto data)
        {
            var errors = new List<validationErrorDto>();

            // Validar Condicion (Contado no permitido)
            if (data.Condicion == "T")
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Condicion",
                    Message = "No se permite la condición de pago 'Contado' (T)."
                });
            }

            // Validar que los números decimales tienen máximo dos dígitos
            decimal[] decimalsToValidate = { data.TotalGravadas, data.TotalExoneradas, data.TotalPercepcion, data.TotalIGV, data.TotalPagar };
            foreach (var dec in decimalsToValidate)
            {
                if (dec != Math.Round(dec, 2))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Total",
                        Message = "El valor decimal debe tener como máximo dos dígitos decimales."
                    });
                }
            }

            // Validar NomTipoDocumento
            if (data.NomTipoDocumento.Length < 5 || data.NomTipoDocumento.Length > 20)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "NomTipoDocumento",
                    Message = "NomTipoDocumento debe tener entre 5 y 20 caracteres"
                });
            }

            // Validar AbrevTipoDocumento
            if (data.AbrevTipoDocumento.Length < 5 || data.AbrevTipoDocumento.Length > 20)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "AbrevTipoDocumento",
                    Message = "AbrevTipoDocumento debe tener entre 5 y 20 caracteres"
                });
            }

            // Validar SerieCompra
            if (data.SerieCompra.Length != 4)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "SerieCompra",
                    Message = "SerieCompra debe tener exactamente 4 caracteres"
                });
            }


            // Validar DocumentoProveedor
            if (data.DocumentoProveedor.Length < 8 || data.DocumentoProveedor.Length > 14)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "DocumentoProveedor",
                    Message = "DocumentoProveedor debe tener entre 8 y 14 caracteres"
                });
            }

            // Validar RazonSocial
            if (data.RazonSocial.Length < 3 || data.RazonSocial.Length > 100)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "RazonSocial",
                    Message = "RazonSocial debe tener entre 3 y 100 caracteres"
                });
            }


            // Validar Moneda
            if (data.Moneda != "N" && data.Moneda != "E")
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Moneda",
                    Message = "Moneda debe ser 'N' o 'E'"
                });
            }

            // Validar Scop
            if (data.Scop.Length != 10)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Scop",
                    Message = "Scop debe tener exactamente 10 caracteres"
                });
            }

            // Validar Compras
            foreach (var compra in data.Compras)
            {
                if (compra.Codigo.Length < 1 || compra.Codigo.Length > 15)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.codigo",
                        Message = "Compras.codigo debe tener entre 1 y 15 caracteres"
                    });
                }

                if (compra.Descripcion.Length < 5 || compra.Descripcion.Length > 200)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.descripcion",
                        Message = "Compras.descripcion debe tener entre 5 y 200 caracteres"
                    });
                }

                if (compra.Api < -99.99m || compra.Api > 99.99m)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.API",
                        Message = "Compras.API debe estar entre -99.99 y 99.99"
                    });
                }

                if (compra.Temp < -10m || compra.Temp > 60m)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.temp",
                        Message = "Compras.temp debe estar entre -10 y 60 grados"
                    });
                }
            }

            return errors;
        }

    }
}
