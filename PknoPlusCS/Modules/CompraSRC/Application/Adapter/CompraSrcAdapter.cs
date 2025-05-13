using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using PknoPlusCS.Global.ApiClient;
using System.Linq;
using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Constantes;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.general;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.bitacora;
using PknoPlusCS.Configuration.Logs;
using System.Windows;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;

namespace PknoPlusCS.Modules.CompraSRC.Application.Adapter
{
    public class CompraSrcAdapter : ICompraSrcInputPort
    {
        private readonly IApiClient _apiClient = new ApiClient();
        private readonly ICompraSrcRepository _compraSrcRepository = new CompraSrcRepository();
        
        public async Task<List<CompraDto>> ObtenerDataSrc()
        {
            var backupRestablecido =  _compraSrcRepository.getBackup();

            Logs.WriteLog("INFO", "Backup "+backupRestablecido.Count());

            if (backupRestablecido.Count == 0) {

                Logs.WriteLog("INFO", "Obteniendo datos del SRC");
                DataStaticDto.data = await obtenerDataDelSrc();
                Logs.WriteLog("INFO", "Termino");

            }
            else
            {

                Logs.WriteLog("INFO", "Obteniendo datos del SRC restablecido");
                DataStaticDto.data = backupRestablecido;

                Logs.WriteLog("INFO", "Iniciando la validacion");
                foreach (var compra in DataStaticDto.data)
                {
                    await validarImportacion(compra.SerieCompra, compra.NumCompra, compra.IdRecepcion);
                }

                //DataStaticDto.data = await obtenerDataDelSrc();
                createBackup();
                Logs.WriteLog("INFO", "termino la validacion de la importacion");

                Logs.WriteLog("INFO", "Iniciadndo validar y bsuqueda de errores en compras " + DataStaticDto.data.Count());

                foreach (var compra in DataStaticDto.data)
                {
                    var errors = await Validations(compra);

                    if (errors.IndError)
                    {
                        compra.Errores = errors;
                        compra.Estado = StatusConstant.Error;
                    }

                }

                Logs.WriteLog("INFO", "Termino de validar y busqueda de errores " + DataStaticDto.data.Count());
            }


            return DataStaticDto.data;
        }

        public void createBackup()
        {
             _compraSrcRepository.crearBackup(DataStaticDto.data);
        }

        public async Task<List<CompraDto>> obtenerDataDelSrc()
        {
            var response = await _apiClient.GetApiDataAsync();


            Logs.WriteLog("INFO", "response> "+response);

            Logs.WriteLog("INFO", "response: "+response.TieneError);

            Logs.WriteLog("INFO", "Resultado: "+response.Resultado);

            Logs.WriteLog("INFO", "response.MensajeError: " + response.MensajeError);
            if (response.TieneError)
            {
                MessageBox.Show(
                    $"Error al consultar la informacion en el SRC: \n\n - Revisa tu conexion a internet\n - Revisa que tengas acceso a la api del SRC \n - vuelve a abrir el formulario \n\n -* error devuelto del api key:\n{response.MensajeError}");
                return new List<CompraDto>();
            }

            if (response.Resultado == null)
            {
                MessageBox.Show("No se encontraron informacion en la api consultada");
                return new List<CompraDto>();
            }

            Logs.WriteLog("INFO", "Se termino la consulta al api de la sunat");

            DataStaticDto.data = response.Resultado;


            foreach (var compra in DataStaticDto.data)
            {
                await Escanear(compra.NumCompra, compra.SerieCompra, compra.DocumentoProveedor);

                await validarImportacion(compra.SerieCompra, compra.NumCompra, compra.IdRecepcion);
            }

            createBackup();

            foreach (var compra in DataStaticDto.data)
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


        public CompraDto ObtenerCompraPorIdRecepcion(string idRecepcion)
        {
            
            var compra = DataStaticDto.data.FirstOrDefault(c => c.IdRecepcion == idRecepcion);
            return compra;
        }

        public  CompraDto ObtenerCompraPorDocumentoProveedor(string documentoProveedor, string codigo)
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
        
            var result =  _compraSrcRepository.GetCliProByRUCOrRazonComercial(data.DocumentoProveedor, data.RazonSocial);

            if (result != null && result.Count > 0)
            {
                data.idCliPro = result[0].IdCliPro;
            }
            else
            {
                var dataSunat = await _apiClient.GetValidSunat(data.DocumentoProveedor);
                 _compraSrcRepository.InsertarCliPro(dataSunat);
                var result1 =  _compraSrcRepository.GetCliProByRUCOrRazonComercial(data.DocumentoProveedor, data.RazonSocial);
                data.idCliPro = result1[0].IdCliPro;
            }

            data.FechaLlegada = data.FechaEmision.ToString();
       

                var dataSucursal = ( _compraSrcRepository.getAllSucursal()).ToList();
                data.NewSucursal = dataSucursal[0].NomPuntoVenta;
                data.SucursalId = dataSucursal[0].IdPuntoVenta;
                data.Sucursal = dataSucursal[0].NomPuntoVenta;
                data.IdAlmacen = Int32.Parse(dataSucursal[0].IdAlmacen);
            
            data.IdPlantilla = ( _compraSrcRepository.spListarEspecificasCompras())[0].IdPlantilla;
            data.NomPlantilla = ( _compraSrcRepository.spListarEspecificasCompras())[0].NomPlantilla;

            data.idTipoOperacion = ( _compraSrcRepository.sp_GetTipoOperacion("02"))[0].IdTipoOperacion;

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

            var data = await _apiClient.GetValidSunat(Credentials.Ruc);
            var sucursal = ( _compraSrcRepository.getAllSucursal()).ToList();

            return new MenuDto
            {
                Ruc = data.NumeroDocumento,
                NomRuc = data.RazonSocial,
                NomSucursal = sucursal[0].NomPuntoVenta
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
                    
                    if(compra.Tratamiento == "10" && compra.Igv <=  0)
                    {
                        genericError.IndError = true;
                        ErrorDetail.Add(new validationErrorDto
                        {
                            Detail = "compra.Igv y compra.Tratamiento",
                            Field = compra.Tratamiento,
                            Message = "Si se trata de una gravada(Operacion gravada) se necesita agregar el IGV"
                        });
                    }

                    if (compra.Tratamiento == "20" && compra.Igv > 0)
                    {
                        genericError.IndError = true;
                        ErrorDetail.Add(new validationErrorDto
                        {
                            Detail = "compra.Igv y compra.Tratamiento",
                            Field = compra.Tratamiento,
                            Message = "Si se trata de una operacion exonerada no debe tener igv"
                        });
                    }


                    data.igvCosto = compra.Tratamiento == "20" ? 1 : 0;

                }

            }

            if(data.NomTipoDocumento == "01")
            {
                data.idClaseDoc = "FAC";
            }
            if (data.TotalPagar <= 0)
            {
                genericError.IndError = true;
                ErrorDetail.Add(new validationErrorDto
                {
                    Detail = "compra.Total",
                    Field = data.TotalPagar,
                    Message = "El monto de la compra no puede ser cero o menor"
                });
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
            data.Moneda = data.Moneda = (data.Moneda.Trim() == "PEN") ? "N" : (data.Moneda.Trim() == "E" || data.Moneda.Trim() == "N" ? data.Moneda.Trim() : "E");
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
            data.pRetencion = data.TotalPercepcion > 0 ? 1: 0;
            data.nCompraPlus = data.SerieCompra + data.NumCompra;
            data.nOrdenCompraProveedor = "0";
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
            var dataPeriodo = ( _compraSrcRepository.ObtenerPeriodosPorFecha(anio, mes))[0];
            var idPeriodo = dataPeriodo.IdPeriodo;

            var compra = DataStaticDto.data.FirstOrDefault(c => c.IdRecepcion == IdRecepcion);
            compra.idPeriodo = idPeriodo;
            compra.cantidad = compra.Compras.Count;

            // await compraSrcRepository.InsertarCompraTemporal(compra);
            foreach (var detalle in compra.Compras)
            {
                detalle.nCompra = compra.SerieCompra + compra.NumCompra;
                 _compraSrcRepository.InsertarCompraTemporal(compra,detalle);
                compra.Estado = StatusConstant.EnProceso;
            }

             _compraSrcRepository.sp_InsertTemporalBitacoraSrc(new TemporalBitacoraSrcDto
            {
                IdRecepcionSrc = compra.IdRecepcion,
                Serie = compra.SerieCompra,
                NumCompra = compra.NumCompra,
                Comentario = "Enviado a migrar al ERP",
                Scop = compra.Scop,
                IdPeriodo = idPeriodo,
                FechaPeriodo = dataPeriodo.FechaFin,
                FiseTotal = compra.Compras.FirstOrDefault().Fise,
            });

            return true;
        }

        public async Task<bool> validarImportacion(string serie,string numCompra,string idRecepcion)
        {
           
            numCompra = numCompra.PadLeft(10, '0');

            var conjunto = "F" + serie + numCompra;

            var data =  _compraSrcRepository.BuscarCompraPorSerieYNumero(serie, conjunto, idRecepcion);

            if(data.Resultado == 1)
            {

                var dataaa= await _apiClient.PutComprobanteAsync(idRecepcion, true);
             
                if (!dataaa.TieneError)
                {

                    var dataModificado = DataStaticDto.data.FirstOrDefault(x => x.IdRecepcion == idRecepcion);
                    dataModificado.Estado = StatusConstant.Migrado;
                    return true;

                }
                else
                {
                    MessageBox.Show("Error al actualizar la factura: " + conjunto + "\n \n - Revisa tu conexion a internet\n - Revisa que tengas acceso a la api del SRC \n - vuelve a abrir el formulario \n\n - error devuelto del api key:\n" + dataaa.MensajeError);
                    return false;
                }
            }

            return false;
        }

        public async Task ActualizarSucursal(string idRecepcion,string IdPuntoVenta,string nomPuntoVenta)
        {
            var dataCompra =  ObtenerCompraPorIdRecepcion(idRecepcion);
            dataCompra.SucursalId = IdPuntoVenta;
            dataCompra.NewSucursal = nomPuntoVenta;
            dataCompra.Sucursal = nomPuntoVenta;
        }

        public async Task<bool> ProductsValidated(string idRecepcion)
        {
            var DataCompra =  ObtenerCompraPorIdRecepcion(idRecepcion);
            foreach(var compra in DataCompra.Compras)
            {
                var data =  _compraSrcRepository.BuscarProductoPorNombreCuencidenciaSrc(compra.Descripcion, DataCompra.DocumentoProveedor);

                if (data.Count > 0) {
                    compra.IdProducto = data[0].IdProductoErp;
                    compra.NomProductoErp = data[0].NombreProdErp;
                    var era =  _compraSrcRepository.getIdProductoExt(compra.IdProducto);

                    if (era.Combustible)
                    {
                        DataCompra.Combustible = true;
                    }
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
        public async Task updateConfiguration(int reiniciar)
        {
             _compraSrcRepository.UpdateConfiguracionInicial(reiniciar);
        }
        public ValidarCierreDto validarCierreArea(DateTime fecha, int idPuntoVenta)
        {
            return _compraSrcRepository.validarCompraCerrado(fecha, idPuntoVenta);

        }
    }
}
