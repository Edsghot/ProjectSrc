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

            DataStaticDto.data = response.data;
            var compraDtos = DataStaticDto.data;
            foreach (var compra in compraDtos)
            {
                var errors = Validations(compra);
                if (errors.Any())
                {
                    compra.Errores = string.Join(", ", errors.Select(e => $"{e.Field}: {e.Message}"));
                }
                compra.Estado = string.IsNullOrEmpty(compra.Errores) ? "Listo" : "No Listo";
            }

            return DataStaticDto.data;
        }

        public async Task<CompraDto> ObtenerCompraPorCodigo(string codigoCompra)
        {
     

            if (DataStaticDto.data == null)
            {
                return null;
            }

            var compra = DataStaticDto.data.FirstOrDefault(c => c.NumCompra == codigoCompra);

            return compra;
        }

        public List<validationErrorDto> Validations(CompraDto data)
        {
            var errors = new List<validationErrorDto>();

            // Validar Condicion (Contado no permitido)
            if (!string.IsNullOrEmpty(data?.Condicion) && data.Condicion == "T")
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Condicion",
                    Message = "No se permite la condición de pago 'Contado' (T)."
                });
            }

            // Validar que los números decimales tienen máximo dos dígitos
            var decimalsToValidate = new[] { data?.TotalGravadas, data?.TotalExoneradas, data?.TotalPercepcion, data?.TotalIGV, data?.TotalPagar };
            foreach (var dec in decimalsToValidate)
            {
                if (dec.HasValue && dec != Math.Round(dec.Value, 2))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Total",
                        Message = "El valor decimal debe tener como máximo dos dígitos decimales."
                    });
                }
            }

            // Validar NomTipoDocumento
            if (!string.IsNullOrEmpty(data?.NomTipoDocumento) && !(data.NomTipoDocumento.Length > 5 && data.NomTipoDocumento.Length < 20))
            {
                errors.Add(new validationErrorDto
                {
                    Field = "NomTipoDocumento",
                    Message = "Debe tener entre 5 y 20 caracteres"
                });
            }

            // Validar AbrevTipoDocumento
            if (!string.IsNullOrEmpty(data?.AbrevTipoDocumento) && !(data.AbrevTipoDocumento.Length >= 2 && data.AbrevTipoDocumento.Length < 10))
            {
                errors.Add(new validationErrorDto
                {
                    Field = "AbrevTipoDocumento",
                    Message = "Debe tener entre 2 y 10 caracteres"
                });
            }

            // Validar SerieCompra
            if (!string.IsNullOrEmpty(data?.SerieCompra) && data.SerieCompra.Length != 4)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "SerieCompra",
                    Message = "Debe tener exactamente 4 caracteres"
                });
            }

            // Validar DocumentoProveedor
            if (!string.IsNullOrEmpty(data?.DocumentoProveedor) && (data.DocumentoProveedor.Length < 8 || data.DocumentoProveedor.Length > 14))
            {
                errors.Add(new validationErrorDto
                {
                    Field = "DocumentoProveedor",
                    Message = "Debe tener entre 8 y 14 caracteres"
                });
            }

            // Validar RazonSocial
            if (!string.IsNullOrEmpty(data?.RazonSocial) && (data.RazonSocial.Length < 3 || data.RazonSocial.Length > 100))
            {
                errors.Add(new validationErrorDto
                {
                    Field = "RazonSocial",
                    Message = "Debe tener entre 3 y 100 caracteres"
                });
            }

            // Validar Moneda
            if (!string.IsNullOrEmpty(data?.Moneda) && (data.Moneda != "N" && data.Moneda != "E"))
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Moneda",
                    Message = "Debe ser 'N' o 'E'"
                });
            }

            // Validar Scop
            if (!string.IsNullOrEmpty(data?.Scop) && data.Scop.Length != 10)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Scop",
                    Message = "Debe tener exactamente 10 caracteres"
                });
            }

            // Validar Compras
            foreach (var compra in data?.Compras ?? Enumerable.Empty<CompraDetalleDto>())
            {
                if (!string.IsNullOrEmpty(compra.Codigo) && (compra.Codigo.Length < 1 || compra.Codigo.Length > 15))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.codigo",
                        Message = "Debe tener entre 1 y 15 caracteres"
                    });
                }

                if (!string.IsNullOrEmpty(compra.Descripcion) && (compra.Descripcion.Length < 5 || compra.Descripcion.Length > 200))
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.descripcion",
                        Message = "Descripcion debe tener entre 5 y 200 caracteres"
                    });
                }

                if (compra.Api < -99.99m || compra.Api > 99.99m)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.API",
                        Message = "Debe estar entre -99.99 y 99.99"
                    });
                }

                if (compra.Temp < -10m || compra.Temp > 60m)
                {
                    errors.Add(new validationErrorDto
                    {
                        Field = "Compras.temp",
                        Message = "Debe estar entre -10 y 60 grados"
                    });
                }
            }

            return errors;
        }
    }
}
