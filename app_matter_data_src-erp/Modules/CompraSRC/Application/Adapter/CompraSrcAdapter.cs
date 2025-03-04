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
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.general;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Constantes;
using System.Windows.Forms.VisualStyles;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.bitacora;

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
                await Escanear(compra.NumCompra, compra.SerieCompra, compra.DocumentoProveedor);
            
                var data = await validarImportacion(compra.SerieCompra, compra.NumCompra, compra.IdRecepcion);

                  
            }

            // Procesar después de que todas las tareas hayan terminado
            foreach (var compra in compraDtos)
            {
                var errors = await Validations(compra);

                if (errors.IndError)
                {
                    compra.Errores = errors;
                    compra.Estado = StatusConstant.Error;
                }
    
            }

            return DataStaticDto.data;
        }


        public async Task<CompraDto> ObtenerCompraPorIdRecepcion(string idRecepcion)
        {
            
            var compra = DataStaticDto.data.FirstOrDefault(c => c.IdRecepcion == idRecepcion);
            return compra;
        }

        public async Task<CompraDto> ObtenerCompraPorDocumentoProveedor(string documentoProveedor, string codigo)
        {
            if (DataStaticDto.data == null)
            {
                return null;
            }

            var partes = codigo.Split('-');
            if (partes.Length != 2)
            {
                return null;
            }

            string serieCompra = partes[0];
            string numCompra = partes[1];

            var compra = DataStaticDto.data.FirstOrDefault(c => c.DocumentoProveedor == documentoProveedor &&
                                                                 c.SerieCompra == serieCompra &&
                                                                 c.NumCompra == numCompra);
            return compra;
        }

        public async Task<string> GetIdRecepcion(string documentoProveedor, string codigo)
        {
            if (DataStaticDto.data == null)
            {
                return null;
            }

            var partes = codigo.Split('-');
            if (partes.Length != 2)
            {
                return null;
            }

            string serieCompra = partes[0];
            string numCompra = partes[1];

            var compra = DataStaticDto.data.FirstOrDefault(c => c.DocumentoProveedor == documentoProveedor &&
                                                                 c.SerieCompra == serieCompra &&
                                                                 c.NumCompra == numCompra);
            return compra.IdRecepcion;
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
        public async Task Escanear(string NumCompra,string serie,string documento)
        {
            var data = DataStaticDto.data.FirstOrDefault(c => c.NumCompra == NumCompra && c.SerieCompra == serie && c.DocumentoProveedor == documento);


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

            data.EstadoProductos = await ProductsValidated(data.IdRecepcion);


            if(data.TotalPercepcion > 0)
            {
                data.seriePer = data.seriePer = new string(data.SerieCompra.Where(char.IsDigit).ToArray()).PadLeft(4, '0');
                data.numPer = data.numPer;
                data.FechaPer = data.FechaPer;
             
            }

            if (!string.IsNullOrWhiteSpace(data.NewSucursal) && !string.IsNullOrWhiteSpace(data.SucursalId))
            {
                data.EstadoSucursal = true;
            }

            if (data.IdAlmacen >= 0 )
            {
                data.EstadoAlmacen = true;
            }

            if(!string.IsNullOrWhiteSpace(data.IdPlantilla) && !string.IsNullOrWhiteSpace(data.NomPlantilla))
            {
                data.EstadoAsiento = true;
            }

            if (!string.IsNullOrWhiteSpace(data.FechaLlegada))
            {
                data.EstadoFechaLlegada = true;
            }

            if(data.EstadoProductos && data.EstadoFechaLlegada && data.EstadoSucursal && data.EstadoAlmacen && data.EstadoAsiento)
            {
                data.Estado = StatusConstant.Listo;
            }
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

        public GenericErrorsDto GetErrorsDetail(string idRecepcion)
        {
            var compra = DataStaticDto.data.FirstOrDefault(c => c.IdRecepcion == idRecepcion);
            return compra.Errores;
        }

        public async Task<GenericErrorsDto> Validations(CompraDto data)
        {
            var genericError = new GenericErrorsDto();
            var HeaderError = new List<validationErrorDto>();
            var ErrorDetail = new List<validationErrorDto>();

            if (string.IsNullOrWhiteSpace(data.IdRecepcion))
            {
                genericError.IndError = false;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Id recepcion",
                    Field = data.IdRecepcion,
                    Message = "No puede ser vacio el IdRecepcion"
                });
            }
            genericError.IdRecepcion = data.IdRecepcion;

            
            // Validar Condicion (Contado no permitido)
            //if (data.IdCondicion != 1)
            //{
            //    genericError.IndError = true;
            //    HeaderError.Add(new validationErrorDto
            //    {
            //        Detail = "Codicion de la factura",
            //        Field = "Condicion",
            //        Message = "No se permite la condición de pago 'Contado' (T)."
            //    });
            //}

            // Validar que los números decimales tienen máximo dos dígitos
            var decimalsToValidate = new[] { data?.TotalGravadas, data?.TotalExoneradas, data?.TotalPercepcion, data?.TotalIGV, data?.TotalPagar };
            foreach (var dec in decimalsToValidate)
            {
                if (dec.HasValue && dec != Math.Round(dec.Value, 2))
                {

                    genericError.IndError = true;
                    HeaderError.Add(new validationErrorDto
                    {
                        Detail = "Decimales de los campos que muestran datos decimales",
                        Field = data.TotalPagar +"",
                        Message = "El valor decimal debe tener como máximo dos dígitos decimales."
                    });
                }
            }


            // Validar AbrevTipoDocumento
            if (string.IsNullOrWhiteSpace(data.NomTipoDocumento) || data.NomTipoDocumento.Length != 2)
            {
                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Tipo de documento",
                    Field = data.NomTipoDocumento,
                    Message = "Debe tener entre 2 y 10 caracteres"
                });
            }

            // Validar SerieCompra
            if (string.IsNullOrWhiteSpace(data.SerieCompra) || data.SerieCompra.Length != 4)
            {

                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Serie de comprobante",
                    Field = data.SerieCompra,
                    Message = "Debe tener exactamente 4 caracteres"
                });
            }

            // Validar que FechaVencimiento no sea menor que FechaEmision
            if (data.FechaVencimiento < data.FechaEmision)
            {

                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Fecha de vencimiento del comprobante",
                    Field = data.FechaVencimiento,
                    Message = "La fecha de vencimiento no puede ser menor que la fecha de emisión"
                });
            }


            // Validar DocumentoProveedor
            if (string.IsNullOrWhiteSpace(data?.DocumentoProveedor) || (data.DocumentoProveedor.Length < 8 || data.DocumentoProveedor.Length > 14))
            {

                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Documento del proveedor (Ruc,Dni)",
                    Field = data.DocumentoProveedor,
                    Message = "Debe tener entre 8 y 14 caracteres"
                });
            }

            // Validar RazonSocial
            if (string.IsNullOrWhiteSpace(data?.RazonSocial))
            {
                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "Razon social del proveedor",
                    Field = data.RazonSocial,
                    Message = "Debe tener entre 3 y 100 caracteres"
                });
            }

            if (data.Compras.Count <= 0)
            {
                genericError.IndError = true;
                HeaderError.Add(new validationErrorDto
                {
                    Detail = "data.Detalle",
                    Field = data.Compras,
                    Message = "debe tener al menos un producto como detalle"
                });
                genericError.HeaderError = HeaderError;
                return genericError;
            }

            // Validar Compras
            foreach (var compra in data?.Compras ?? Enumerable.Empty<CompraDetalleDto>())
            {

                if ((string.IsNullOrWhiteSpace(compra.Descripcion) && ( (compra.Descripcion.Length < 2 || compra.Descripcion.Length > 200))))
                {
                    genericError.IndError = true;
                    ErrorDetail.Add(new validationErrorDto
                    {
                        Detail = "Descripcion de la compra (Nombre del producto)",
                        Field = compra.Descripcion,
                        Message = "Descripcion debe tener entre 5 y 200 caracteres"
                    });
                }
                if (compra.Cantidad <= 0)
                {
                    genericError.IndError = true;
                    ErrorDetail.Add(new validationErrorDto
                    {
                        Detail = "Cantidad de productos",
                        Field = compra.Cantidad,
                        Message = "La cantidad de productos debe ser mayor a cero"
                    });
                }
   

                if (string.IsNullOrWhiteSpace(compra.Tratamiento) &&( compra.Tratamiento != "10" || compra.Tratamiento != "30"))
                {
                    genericError.IndError = true;
                    ErrorDetail.Add(new validationErrorDto
                    {
                        Detail = "Tratamiento de la compra",
                        Field = compra.Tratamiento,
                        Message = "Tienes que enviar el codigo de tratamiento ya sea 10 o 30 (exonerado o inafecto)"
                    });
                    
                    if(compra.Tratamiento == "10" && compra.Igv > 0)
                    {
                        genericError.IndError = true;
                        ErrorDetail.Add(new validationErrorDto
                        {
                            Detail = "compra.Igv y compra.Tratamiento",
                            Field = compra.Tratamiento,
                            Message = "Si se trata de una gravada(Operacion onerosa) se necesita agregar el IGV"
                        });
                    }
                
                }

            }

            if(data.NomTipoDocumento == "01")
            {
                data.idClaseDoc = "FAC";
            }

            if (data.NomTipoDocumento != "01")
            {
                genericError.IndError = true;
                ErrorDetail.Add(new validationErrorDto
                {
                    Detail = "Tipo de documento no manejadno en esta aplicacion",
                    Field = data.NomTipoDocumento,
                    Message = "Revisa si se trata de un factura"
                });
            }
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

            genericError.HeaderError = HeaderError;
            genericError.ErrorDetail = ErrorDetail;
            return genericError;
        }

        public async Task<bool> InsertCompra(int mes, int anio, string IdRecepcion)
        {
            var dataPeriodo = (await compraSrcRepository.ObtenerPeriodosPorFecha(anio, mes))[0];
            var idPeriodo = dataPeriodo.IdPeriodo;

            var compra = DataStaticDto.data.FirstOrDefault(c => c.IdRecepcion == IdRecepcion);
            compra.idPeriodo = idPeriodo;
            compra.cantidad = compra.Compras.Count;

           // await compraSrcRepository.InsertarCompraTemporal(compra);
            foreach (var detalle in compra.Compras)
            {
                detalle.nCompra = compra.SerieCompra + compra.NumCompra;
                await compraSrcRepository.InsertarCompraTemporal(compra,detalle);
                compra.Estado = StatusConstant.EnProceso;
            }

            await compraSrcRepository.sp_InsertTemporalBitacoraSrc(new TemporalBitacoraSrcDto
            {
                IdRecepcionSrc = compra.IdRecepcion,
                Serie = compra.SerieCompra,
                NumCompra = compra.NumCompra,
                Comentario = "Enviado a migrar al ERP",
                Scop = compra.Scop,
                IdPeriodo = idPeriodo,
                FechaPeriodo = dataPeriodo.FechaFin,
                FiseTotal = compra.fiseTotal,
            });

            return true;
        }

        public async Task<bool> validarImportacion(string serie,string numCompra,string idRecepcion)
        {
            numCompra = new string(numCompra.Where(c => char.IsDigit(c) && c != '0').ToArray()); // Elimina los ceros

            numCompra = numCompra.PadLeft(8, '0');

            var data = await compraSrcRepository.BuscarCompraPorSerieYNumero(serie, numCompra,idRecepcion);

            if(data.Resultado == 1)
            {

                var dataaa= await apiClient.PutComprobanteAsync(idRecepcion, true);
                //DataStaticDto.data.RemoveAll(x => x.IdRecepcion == idRecepcion);

                return true;

            }

            var dataaa2 = await apiClient.PutComprobanteAsync(idRecepcion, false);
            return false;
        }

        public async Task ActualizarSucursal(string idRecepcion,string IdPuntoVenta,string nomPuntoVenta)
        {
            var dataCompra = await ObtenerCompraPorIdRecepcion(idRecepcion);
            dataCompra.SucursalId = IdPuntoVenta;
            dataCompra.NewSucursal = nomPuntoVenta;
            dataCompra.Sucursal = nomPuntoVenta;
        }

        public async Task<bool> ProductsValidated(string idRecepcion)
        {
            var DataCompra = await ObtenerCompraPorIdRecepcion(idRecepcion);
            foreach(var compra in DataCompra.Compras)
            {
                var data = await compraSrcRepository.BuscarProductoPorNombreCuencidenciaSrc(compra.Descripcion, DataCompra.DocumentoProveedor);

                if (data.Count > 0) {
                    compra.IdProducto = data[0].IdProductoErp;
                    compra.NomProductoErp = data[0].NombreProdErp;
                }
                if (data.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task ActualizarScopApiTemp(string idRecepcion,string nomProducto, string scop, decimal api,decimal temp)
        {
            var data = DataStaticDto.data.FirstOrDefault(x => x.idCompraSerie == ExtraStatic.idRecepcion);
            data.Scop = scop;

            var dataProd = data.Compras.FirstOrDefault(x => x.Descripcion == nomProducto);
            dataProd.Api = api;
            dataProd.Temp = temp;
        }
    }
}
