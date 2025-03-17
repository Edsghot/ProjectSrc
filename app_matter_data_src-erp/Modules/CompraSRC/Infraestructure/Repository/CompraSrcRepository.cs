using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using app_matter_data_src_erp.Configuration.Constants;
using app_matter_data_src_erp.Global.DataBase;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.bitacora;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Configuracion;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Proveedor;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using ExpressMapper;

namespace app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository
{
    public class CompraSrcRepository : ICompraSrcRepository
    {
        public async Task<IEnumerable<ProductDto>> searchProducts(string nameProduct)
        {

            var parameters = new[]
            {
                new SqlParameter("@NombreProducto", nameProduct)
            };

            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("spBuscarProductoCercano", parameters);

            var products = data.AsEnumerable().Select<DataRow, ProductDto>(row => Mapper.Map<DataRow, ProductDto>(row)).ToList();

            return products;

        }


        public async Task InsertProdCuencidencia(InsertProdCuencidenciaDto data)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdProductoErp", data.IdProductoErp),
                new SqlParameter("@NombreProdErp", data.NombreProdErp),
                new SqlParameter("@NombreProdSrc", data.NombreProdSrc),
                new SqlParameter("@RucEmpresa", data.RucEmpresa)
            };
            await DataBaseHelper.ExecuteStoredProcedureAsync("spInsertarOActualizarCoincidenciaProdSrc", parameters);
        }

        public async Task<IEnumerable<SucursalDto>> getAllSucursal()
        {
            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("spObtenerPuntosVentaConAlmacen");

            var puntosVentaConAlmacen = result.AsEnumerable()
                .Select<DataRow, SucursalDto>(row => Mapper.Map<DataRow, SucursalDto>(row))
                .ToList();

            return puntosVentaConAlmacen;
        }

        public async Task<List<CoincidenciaProdSrcDto>> ObtenerCoincidenciasProdSrcPorRuc(string rucEmpresa)
        {
            var parameters = new[]
            {

                new SqlParameter("@RucEmpresa", rucEmpresa)
             };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("spObtenerCoincidenciasProdSrcPorRuc", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CoincidenciaProdSrcDto>(row => Mapper.Map<DataRow, CoincidenciaProdSrcDto>(row))
                .ToList();

            return coincidencias;
        }
        public async Task<List<CoincidenciaProdSrcDto>> BuscarProductoPorNombreCuencidenciaSrc(string NombreProd,string rucEmpresa)
        {
            var parameters = new[]
            {
                new SqlParameter("@NombreProducto", NombreProd),
                new SqlParameter("@Ruc", rucEmpresa)
             };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("spBuscarProductoPorNombreCuencidenciaSrc", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CoincidenciaProdSrcDto>(row => Mapper.Map<DataRow, CoincidenciaProdSrcDto>(row))
                .ToList();

            return coincidencias;
        }

        public async Task<ValidarImpoDto> BuscarCompraPorSerieYNumero(string serie,string compra,string idRecepcion)
        {
            var parameters = new[]
            {

                new SqlParameter("@serie", serie),
                new SqlParameter("@numCompra", compra),
                new SqlParameter("@idRecepcion", idRecepcion)
            };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_BuscarCompraPorSerieYNumero", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, ValidarImpoDto>(row => Mapper.Map<DataRow, ValidarImpoDto>(row))
                .ToList();

            return coincidencias.FirstOrDefault() ?? new ValidarImpoDto(); 
        }

        public async Task<List<CliProveedorDto>> GetCliProByRUCOrRazonComercial(string rucEmpresa, string razonSocial)
        {
            var parameters = new[]
            {
                new SqlParameter("@RUC", rucEmpresa)
            };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_GetCliProByRUCRazonComercial", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CliProveedorDto>(row => Mapper.Map<DataRow, CliProveedorDto>(row))
                .ToList();

            return coincidencias;
        }


        public async Task<List<ClaseTipoSunatDto>> GetClaseDocByTipoSunat(string tipoSunat)
        {
            var parameters = new[]
            {
                new SqlParameter("@TipoSunat", tipoSunat)
            };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_GetClaseDocByTipoSunat", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, ClaseTipoSunatDto>(row => Mapper.Map<DataRow, ClaseTipoSunatDto>(row))
                .ToList();

            return coincidencias;
        }

        public async Task<List<PeriodoDto>> ObtenerPeriodosPorFecha(int anio, int mes)
        {
            var parameters = new[]
            {
                new SqlParameter("@Anio", anio),
                new SqlParameter("@Mes", mes)

            };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_ObtenerPeriodosPorFecha", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, PeriodoDto>(row => Mapper.Map<DataRow, PeriodoDto>(row))
                .ToList();

            return coincidencias;
        }

        public async Task<List<PlantillasDto>> spListarEspecificasCompras()
        {

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("spListarEspecificasCompras");

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, PlantillasDto>(row => Mapper.Map<DataRow, PlantillasDto>(row))
                .ToList();

            return coincidencias;
        }

        public async Task<List<TipoOperacionDto>> sp_GetTipoOperacion(string codSunat)
        {

            var parameters = new[]
             {
                new SqlParameter("@codSunat", codSunat),

            };

            var result = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_GetTipoOperacion", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, TipoOperacionDto>(row => Mapper.Map<DataRow, TipoOperacionDto>(row))
                .ToList();

            return coincidencias;
        }

        public async Task ActualizarPuntoVentaYAlmacen(int idPuntoVenta, int idAlmacen)
        {
            var parameters = new[]
            {
                    new SqlParameter("@IdPuntoVenta", idPuntoVenta),
                    new SqlParameter("@IdAlmacen", idAlmacen)
                };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spActualizarPuntoVentaYAlmacenSrc", parameters);
        }

        public async Task sp_InsertTemporalBitacoraSrc(TemporalBitacoraSrcDto data)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", data.IdRecepcionSrc),
                new SqlParameter("@serie", data.Serie),
                new SqlParameter("@numCompra", data.NumCompra),
                new SqlParameter("@comentario", data.Comentario),
                new SqlParameter("@fecha", DateTime.Now),
                new SqlParameter("@scop", data.Scop),
                new SqlParameter("@idPeriodo", data.IdPeriodo ),
                new SqlParameter("@fechaPeriodo", data.FechaPeriodo ),
                new SqlParameter("@fiseTotal", data.FiseTotal)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("sp_InsertTemporalBitacoraSrc", parameters);
        }

        public async Task UpdateConfiguracionInicial(int  reiniciar)
        {
            var parameters = new[]
            {
                new SqlParameter("@reiniciar", reiniciar)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("Sp_UpdateConfiguracionFormSrc", parameters);
        }

        public async Task InsertarDetalleTemporalSrc(DetalleTemporalBitacoraSrcDto data)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", data.IdRecepcionSrc),
                new SqlParameter("@idProductoSrc", data.IdProductoSrc),
                new SqlParameter("@nomProductoSrc", data.NomProductoSrc),
                new SqlParameter("@api", data.Api),
                new SqlParameter("@temp", data.Temp),
                new SqlParameter("@fecha", DateTime.Now)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("DetalleTemporalSrc", parameters);
        }


        public async Task<IEnumerable<ProductDto>> BuscarProductoPorId(string idProducto)
        {
            var parameters = new[]
            {
        new SqlParameter("@IdProducto", idProducto)};

            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("spBuscarProductoPorId", parameters);

            var products = data.AsEnumerable()
                .Select<DataRow, ProductDto>(row => Mapper.Map<DataRow, ProductDto>(row))
                .ToList();

            return products;
        }


        public async Task InsertarCliPro(ProveedorDto proveedor)
        {
            var parameters = new[]
            {
                new SqlParameter("@NomCliPro", proveedor.RazonSocial),
                new SqlParameter("@Tipo", 'P'),
                new SqlParameter("@RazonComercial", proveedor.RazonSocial),
                new SqlParameter("@IdClaseDocDef", 1), // Valor por defecto
                new SqlParameter("@RUC", proveedor.NumeroDocumento),
                new SqlParameter("@IdTipoDI", "RUC"), // Valor por defecto
                new SqlParameter("@DNI", proveedor.NumeroDocumento), // Valor por defecto
                new SqlParameter("@Sexo", 'S'), // Valor por defecto
                new SqlParameter("@IdPuntoVenta", Credentials.IdPuntoVenta), // Valor por defecto
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("sp_InsertCliPro", parameters);
        }

        public async Task ActualizarProductoCompraTemporalMonitoreoSRC(string idProducto,string numCompra,string serieCompra,string NomProducto,decimal api,decimal temp,string scop)
        {
            var parameters = new[]
            {
                new SqlParameter("@idProducto", idProducto),
                new SqlParameter("@numCompra", numCompra),
                new SqlParameter("@SerieCompra", serieCompra),
                new SqlParameter("@nomProducto", NomProducto),
                new SqlParameter("@api", api),
                new SqlParameter("@temp", temp),
                new SqlParameter("@scop",scop)
                // Valor por defecto
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spActualizarCompraTemporalMonitoreoSRC", parameters);
        } 
        public async Task ActualizaCabeceraTemporalMonitoreoSRC(string idRecepcion,int idSucursal,int IdPeriodo)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcion", idRecepcion),
                new SqlParameter("@idSucursal", idSucursal),
                new SqlParameter("@idPeriodo", IdPeriodo)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spActualizaCabeceraTemporalMonitoreoSRC", parameters);
        } 
        public async Task InsertarEliminarComprobanteSrc(string idRecepcionSrc,string nCompra,int idPeriodo,DateTime fechaLlegada,string scop)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", idRecepcionSrc),
                new SqlParameter("@nCompraErp", nCompra),
                new SqlParameter("@idPeriodo", idPeriodo),
                new SqlParameter("@fecha", DateTime.Now),
                new SqlParameter("@fechaLlegada", fechaLlegada),
                new SqlParameter("@nOrdenCompra", scop),
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spInsertarEliminarComprobanteSrc", parameters);
        }

        public async Task<List<CompraTemporalMonitoreoSrcDto>> ObtenerCompraTemporalMonitoreoSrc(int? estado = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@estado", estado ?? (object)DBNull.Value)
            };

            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("spObtenerCompraTemporalMonitoreoSRC", parameters);

            var compraMonitoreos = data.AsEnumerable()
                .Select<DataRow, CompraTemporalMonitoreoSrcDto>(row => Mapper.Map<DataRow, CompraTemporalMonitoreoSrcDto>(row))
                .ToList();

            return compraMonitoreos;
        }



        public async Task<List<CompraTemporalMonitoreoSrcDto>> ObtenerCompraMonitoreoTemporalPorIdRecepcion(string idRecepcion)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", idRecepcion)
            };

            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("sp_ObtenerCompraTemporalMonitoreo", parameters);

            var compraMonitoreos = data.AsEnumerable()
                .Select<DataRow, CompraTemporalMonitoreoSrcDto>(row => Mapper.Map<DataRow, CompraTemporalMonitoreoSrcDto>(row))
                .ToList();

            return compraMonitoreos;
        }
        public async Task InsertarCompraTemporalActualizar(CompraTemporalMonitoreoSrcDto data)
        {
            
            var parameters = new[]
            {
                new SqlParameter("@idComputadora", Credentials.IdComputadora),
                new SqlParameter("@tipoDoc", "FAC"),
                new SqlParameter("@serieCompra", data.SerieCompra),
                new SqlParameter("@numCompra", data.NumCompra),

                new SqlParameter("@rucPersona", data.RucPersona),
                new SqlParameter("@nomPersona", data.NomPersona),
                new SqlParameter("@sucursal", data.Sucursal),
                new SqlParameter("@fecha", data.Fecha),
                new SqlParameter("@fechaVenc", data.FechaVenc),
                new SqlParameter("@moneda", data.Moneda),
                new SqlParameter("@tcCompra", 3.5),
                new SqlParameter("@condicion", "R"),
                new SqlParameter("@nomTransportista",  string.Empty),
                new SqlParameter("@rucTransportista", string.Empty),
                new SqlParameter("@dirTransportista", string.Empty),
                new SqlParameter("@placaTransportista", string.Empty),
                new SqlParameter("@marcaTransportista",  string.Empty),
                new SqlParameter("@certInscripcion", string.Empty),
                new SqlParameter("@configuracionVeh", string.Empty),
                new SqlParameter("@cubicacion", string.Empty),
                new SqlParameter("@nomChofer", string.Empty),
                new SqlParameter("@breveteChofer", string.Empty),
                new SqlParameter("@destinoRC", 10),
                new SqlParameter("@obs", "Migrado desde el SRC"),
                new SqlParameter("@subTotal", data.SubTotal),
                new SqlParameter("@igv", data.Igv),
                new SqlParameter("@total", data.Total),
                new SqlParameter("@nDetraccion", string.Empty),
                new SqlParameter("@fDetraccion",  (object)DBNull.Value),
                new SqlParameter("@fechaLlegada", data.FechaLlegada ?? (object)DBNull.Value),
                new SqlParameter("@precioIncluyeIGV", data.PrecioIncluyeIGV),
                new SqlParameter("@tipoOperacion", 2),
                new SqlParameter("@centroCostos", data.Sucursal),
                new SqlParameter("@seriePer", string.IsNullOrEmpty(data.SeriePer) ? (object)DBNull.Value : data.SeriePer),
                new SqlParameter("@numPer", string.IsNullOrEmpty(data.NumCompra) ? (object)DBNull.Value : data.NumCompra),
                new SqlParameter("@fechaPercepcion", (object)DBNull.Value),
                new SqlParameter("@perTotal", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = 0.00m },
                new SqlParameter("@pRetencion",SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = 0.00m },
                new SqlParameter("@validarTotales", false),
                new SqlParameter("@idProductoExt",data.IdProductoExt),
                new SqlParameter("@cantidad", data.Cantidad),
                new SqlParameter("@precio", data.Precio),
                new SqlParameter("@tipoIGV", 28),
                new SqlParameter("@pIGV", 0.18),
                new SqlParameter("@fechaVencProducto", (object) DBNull.Value),
                new SqlParameter("@api", data.Api),
                new SqlParameter("@temperatura", data.Temperatura),
                new SqlParameter("@igvCosto", 0.00m),
                new SqlParameter("@serieProducto", string.Empty),
                //mostrando......................................................
                new SqlParameter("@NomProductoSrc", data.NomProductoSrc),
                new SqlParameter("@IdRecepcionSrc", data.IdRecepcionSrc),
                new SqlParameter("@IdPeriodo", data.IdPeriodo)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spInsertCompraTemporalSRC", parameters);


        }

        public async Task InsertarCompraTemporal(CompraDto compra, CompraDetalleDto dCompra)
        {
            compra.Condicion = "R";

            if (compra.FechaEmision.Date == compra.FechaVencimiento.Date)
            {
                compra.FechaVencimiento = compra.FechaVencimiento.AddDays(1);
            }


            var parameters = new[]
            {
                new SqlParameter("@idComputadora", Credentials.IdComputadora),
                new SqlParameter("@tipoDoc", "FAC"),
                new SqlParameter("@serieCompra", compra.SerieCompra ?? string.Empty),
                new SqlParameter("@numCompra", compra.NumCompra?.TrimStart('0').PadLeft(8, '0') ?? string.Empty),
    
                new SqlParameter("@rucPersona", compra.DocumentoProveedor ?? string.Empty),
                new SqlParameter("@nomPersona", compra.RazonSocial ?? string.Empty),
                new SqlParameter("@sucursal", compra.SucursalId ?? string.Empty),
                new SqlParameter("@fecha", compra.FechaEmision),
                new SqlParameter("@fechaVenc", compra.FechaVencimiento),
                new SqlParameter("@moneda", compra.Moneda ?? string.Empty),
                new SqlParameter("@tcCompra", compra.TipoCambio),
                new SqlParameter("@condicion", compra.Condicion ?? string.Empty),
                new SqlParameter("@nomTransportista", compra.LicenciaTransportista ?? string.Empty),
                new SqlParameter("@rucTransportista", string.Empty),
                new SqlParameter("@dirTransportista", string.Empty),
                new SqlParameter("@placaTransportista", compra.PlacaTransportista ?? string.Empty),
                new SqlParameter("@marcaTransportista", compra.MarcaTransportista ?? string.Empty),
                new SqlParameter("@certInscripcion", string.Empty),
                new SqlParameter("@configuracionVeh", string.Empty),
                new SqlParameter("@cubicacion", string.Empty),
                new SqlParameter("@nomChofer", string.Empty),
                new SqlParameter("@breveteChofer", string.Empty),
                new SqlParameter("@destinoRC", 10),
                new SqlParameter("@obs", string.IsNullOrEmpty(compra.Observacion) ? "-" : compra.Observacion),
                new SqlParameter("@subTotal", compra.TotalPagar-compra.TotalIGV),
                new SqlParameter("@igv", compra.TotalIGV),
                new SqlParameter("@total", compra.TotalPagar),
                new SqlParameter("@nDetraccion", string.Empty),
                new SqlParameter("@fDetraccion",  (object)DBNull.Value),
                new SqlParameter("@fechaLlegada", compra.FechaLlegada ?? (object)DBNull.Value),
                new SqlParameter("@precioIncluyeIGV", compra.PrecioIncluyeIGV),
                new SqlParameter("@tipoOperacion", compra.idTipoOperacion),
                new SqlParameter("@centroCostos", compra.SucursalId),
                new SqlParameter("@seriePer", string.IsNullOrEmpty(compra.seriePer) ? (object)DBNull.Value : compra.seriePer),
                new SqlParameter("@numPer", string.IsNullOrEmpty(compra.NumCompra) ? (object)DBNull.Value : compra.NumCompra),
                new SqlParameter("@fechaPercepcion", string.IsNullOrEmpty(compra.FechaPer) ? (object)DBNull.Value : (object)DateTime.Parse(compra.FechaPer)),
                new SqlParameter("@perTotal", compra.TotalPercepcion != null ? compra.TotalPercepcion : 0.00m),
                new SqlParameter("@pRetencion", compra.pRetencion),
                new SqlParameter("@validarTotales", false),
                new SqlParameter("@idProductoExt", dCompra.IdProducto),
                new SqlParameter("@cantidad", dCompra.Cantidad),
                new SqlParameter("@precio", dCompra.PrecioUnitarioConIgv),
                new SqlParameter("@tipoIGV", 28),
                new SqlParameter("@pIGV", 0.18),
                new SqlParameter("@fechaVencProducto", (object)compra.FechaVencimiento ?? DBNull.Value),
                new SqlParameter("@api", dCompra.Api),
                new SqlParameter("@temperatura", dCompra.Temp),
                new SqlParameter("@igvCosto", 0.00m),
                new SqlParameter("@serieProducto", string.Empty),
                //mostrando......................................................
                new SqlParameter("@NomProductoSrc", dCompra.Descripcion),
                new SqlParameter("@IdRecepcionSrc", compra.IdRecepcion),
                new SqlParameter("@IdPeriodo", compra.idPeriodo)
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spInsertCompraTemporal", parameters);

           
        }

        public async Task<GetConfiguracionDto> GetConfiguracionInicial()
        {
          
            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("Sp_GetConfigurationSrc");

            var configuracion = data.AsEnumerable()
                .Select<DataRow, GetConfiguracionDto>(row => Mapper.Map<DataRow, GetConfiguracionDto>(row))
                .ToList();
            if(configuracion.Count <= 0)
            {
                return new GetConfiguracionDto();
            }
            return configuracion[0];
        }
        public async Task<GetProductExtDto> getIdProductoExt(string idProducto)
        {
            var parameters = new[]
            {
                new SqlParameter("@idProducto", idProducto)
            };
            var data = await DataBaseHelper.ExecuteStoredProcedureAsync("Sp_GetIdProductoExt",parameters);

            var configuracion = data.AsEnumerable()
                .Select<DataRow, GetProductExtDto>(row => Mapper.Map<DataRow, GetProductExtDto>(row))
                .ToList();
            if (configuracion.Count <= 0)
            {
                return new GetProductExtDto();
            }
            return configuracion[0];
        }

        private int GetMaxLength(string paramName)
        {
            switch (paramName)
            {
                case "@nCompra": return 15;
                case "@idCliPro": return 30;
                case "@idClaseDoc": return 1;
                case "@Moneda": return 1;
                case "@Condicion": return 1;
                case "@NGuiaRemision": return 20;
                case "@numConstanciaDep": return 20;
                case "@nOrdenCompra": return 15;
                case "@nProcesoAsociado": return 15;
                case "@nPercepcion": return 15;
                case "@nCompraPlus": return 20;
                case "@nOrdenCompraProveedor": return 17;
                default: return -1; // Retorna -1 si no hay un límite definido
            }
        }
    }
}