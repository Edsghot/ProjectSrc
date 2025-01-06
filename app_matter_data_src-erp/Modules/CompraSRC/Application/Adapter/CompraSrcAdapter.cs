using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using app_matter_data_src_erp.Configuration.Constants;
using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Global.DtoGlobales;
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

    
        public async Task ObtenerDataSrc()
        {
            var data = await apiClient.GetApiDataAsync();
            var dataa = data.data;

            var errors = ValidateData(dataa[0]);

            return;
        }
        public List<validationErrorDto> ValidateData(CompraDto data)
        {
            var errors = new List<validationErrorDto>();

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

            // Validar NumCompra
            if (data.NumCompra.Length != 10)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "NumCompra",
                    Message = "NumCompra debe tener exactamente 10 caracteres"
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

            // Validar Sucursal
            if (data.Sucursal.Length < 5 || data.Sucursal.Length > 150)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Sucursal",
                    Message = "Sucursal debe tener entre 5 y 150 caracteres"
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

            // Validar Condicion
            if (data.Condicion != "R" && data.Condicion != "T")
            {
                errors.Add(new validationErrorDto
                {
                    Field = "Condicion",
                    Message = "Condicion debe ser 'R' o 'T'"
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