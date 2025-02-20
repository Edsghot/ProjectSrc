using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using System.Linq;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Data;
using app_matter_data_src_erp.Configuration.Constants;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Static;
using System.Windows.Markup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter
{
    public class CompraSrcAdapter : ICompraSrcInputPort
    {
        private readonly IApiClient apiClient;
        private readonly ICompraSrcRepository compraSrcRepository;

        public CompraSrcAdapter()
        {
            this.apiClient = new ApiClient();
            compraSrcRepository = new CompraSrcRepository();
        }

        public async Task<List<CompraDto>> ObtenerDataSrc()
        {
            var response = await apiClient.GetApiDataAsync();

            if (response == null || response.Resultado == null)
            {
                return new List<CompraDto>();
            }

            DataStaticDto.data = response.Resultado;
            var compraDtos = DataStaticDto.data;

            
            foreach (var compra in compraDtos)
            {
                await Escanear(compra.NumCompra);
            }

            // Procesar después de que todas las tareas hayan terminado
            foreach (var compra in compraDtos)
            {
                var errors = await Validations(compra);

                if (errors.Any())
                {
                    compra.Errores = "Error: " + string.Join(", ", errors.Select(e => $"{e.Field}: {e.Message}"));
                    compra.Estado = "Error";
                }
                else
                {
                    compra.Errores = "No Listo";
                    compra.Estado = "No Listo";
                }

                var validateCompra = (await validarImportacion(compra.SerieCompra, compra.NumCompra));

                if (validateCompra)
                {
                    compra.Estado = "Importado";
                    compra.Errores = "Importado";
                    compra.Coicidencia = "Revisado";
                }

            }

            return DataStaticDto.data;
        }


        public async Task<CompraDto> ObtenerCompraPorCodigo(string codigoCompra)
        {


            if (DataStaticDto.data == null)
            {
                return null;
            }

            var compra = DataStaticDto.data.FirstOrDefault(c => c.idCompraSerie == codigoCompra);

            return compra;
        }

        public async Task EscanearDCompra(string IdProduct, string NombreProducto)
        {
            foreach (var compra in DataStaticDto.data)
            {
                foreach (var detalle in compra.Compras)
                {
                    if (detalle.Descripcion == NombreProducto)
                    {
                        detalle.IdProducto = IdProduct;
                    }
                }
            }
        }
        public async Task Escanear(string NumCompra)
        {
            var data = DataStaticDto.data.FirstOrDefault(c => c.NumCompra == NumCompra);


            data.idCompraSerie = data.SerieCompra + "-" + data.NumCompra;
        
            var result = await compraSrcRepository.GetCliProByRUCOrRazonComercial(data.DocumentoProveedor, data.RazonSocial);

            if (result != null && result.Count > 0)
            {
                data.idCliPro = result[0].IdCliPro;
            }
            else
            {
                var dataSunat = await apiClient.GetValidSunat(data.DocumentoProveedor);
                await compraSrcRepository.InsertarCliPro(dataSunat);
                var result1 = await compraSrcRepository.GetCliProByRUCOrRazonComercial(data.DocumentoProveedor, data.RazonSocial);
                data.idCliPro = result1[0].IdCliPro;
            }

            data.FechaLlegada = data.FechaEmision.ToString();
            var dataSucursal = (await compraSrcRepository.getAllSucursal()).FirstOrDefault(x => x.SucursalSRC == "True");
            data.NewSucursal = dataSucursal.NomPuntoVenta;
            data.SucursalId = dataSucursal.IdPuntoVenta;
            data.Sucursal = dataSucursal.NomPuntoVenta;
            data.IdAlmacen = Int32.Parse(dataSucursal.IdAlmacen);
            data.IdPlantilla = (await compraSrcRepository.spListarEspecificasCompras())[0].IdPlantilla;
            data.NomPlantilla = (await compraSrcRepository.spListarEspecificasCompras())[0].NomPlantilla;

            data.idTipoOperacion = (await compraSrcRepository.sp_GetTipoOperacion("02"))[0].IdTipoOperacion;



        }

        public async Task<MenuDto> GetMenu()
        {

            var data = await apiClient.GetValidSunat(Credentials.Ruc);
            var sucursal = (await compraSrcRepository.getAllSucursal()).ToList().FirstOrDefault(x => x.SucursalSRC == "True");

            return new MenuDto
            {
                Ruc = data.NumeroDocumento,
                NomRuc = data.RazonSocial,
                NomSucursal = sucursal.NomPuntoVenta
            };
        }


        public async Task<List<validationErrorDto>> Validations(CompraDto data)
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

            // Validar que FechaVencimiento no sea menor que FechaEmision
            if (data.FechaVencimiento < data.FechaEmision)
            {
                errors.Add(new validationErrorDto
                {
                    Field = "FechaVencimiento",
                    Message = "La fecha de vencimiento no puede ser menor que la fecha de emisión"
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


            }
            data.idClaseDoc = "FAC";
            data.FechaDig = DateTime.Now;
            data.FechaOperativa = DateTime.Now;
            data.TipoCambio = 3.5M;
            data.NGuiaRemision = data.GuiaRemisionAsociada;
            data.idTransportista = 1;
            data.idPlaca = 1;
            data.idChofer = 1;
            data.Moneda = (data.Moneda.Trim() != "PEN") ? "E" : "N";
            data.idDepartamento = Credentials.IdDepartamento;
            //data.nOrdenCompra 
            data.detraccion = 0;
            data.tieneConsignaciones = false;
            data.fleteTotal = 0;
            data.distribuir = false;
            data.idProcesoAsociado = 0;
            data.guiaRecibida = -1;
            data.nPercepcion = "0";
            //data.Fecha{Percepcion
            data.pRetencion = 0;
            data.nCompraPlus = data.SerieCompra + data.NumCompra;
            data.nOrdenCompraProveedor = "0";
            data.fiseTotal = 0;
            data.idClasificacionBienesServicios = 1;
            data.idTipoFacturacionGuiaRemision = 2;
            data.nProcesoAsociado = "0";
            data.SubTotal = data.TotalPagar;
            data.Importacion = true;
            data.Automatica = true;
            data.IdTurno = Credentials.IdTurno;
            data.RelGuiaCompra = false;
            data.PrecioIncluyeIGV = data.TotalIGV > 0 ? true : false;
            //data.fechaEspecialRC
            data.servicioIntangible = false;
            return errors;
        }

        public async Task<bool> InsertCompra(int mes, int anio, string numCompra)
        {
            var idPeriodo = (await compraSrcRepository.ObtenerPeriodosPorFecha(anio, mes))[0].IdPeriodo;

            var compra = DataStaticDto.data.FirstOrDefault(c => c.idCompraSerie == numCompra);
            compra.idPeriodo = idPeriodo;
            compra.cantidad = compra.Compras.Count;
           // await compraSrcRepository.InsertarCompraTemporal(compra);
            foreach (var detalle in compra.Compras)
            {
                detalle.nCompra = compra.SerieCompra + compra.NumCompra;
                await compraSrcRepository.InsertarCompraTemporal(compra,detalle);
                compra.Estado = "En Proceso";
                compra.Errores = "En Proceso";
            }
            return true;
        }

        public async Task<bool> validarImportacion(string serie,string numCompra)
        {
            numCompra = new string(numCompra.Where(c => char.IsDigit(c) && c != '0').ToArray());

            var data = await compraSrcRepository.BuscarCompraPorSerieYNumero(serie, numCompra);

            if(data.Resultado == 1)
            {
                return true;
            }
            return false;
        }


        public async Task<List<CompraTemporalMonitoreoSrcDto>> ListarImportados(int estatus)
        {
            var data = await compraSrcRepository.ObtenerCompraTemporalMonitoreoSrc(estatus);

            return data;
        }


    }
}
