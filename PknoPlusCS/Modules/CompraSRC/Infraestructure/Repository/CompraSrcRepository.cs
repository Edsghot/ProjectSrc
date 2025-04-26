using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using PknoPlusCS.Configuration.Constants;
using PknoPlusCS.Global.DataBase;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.bitacora;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Configuracion;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Proveedor;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using ExpressMapper;
using PknoPlusCS.Global.Helper;
using RestSharp.Extensions;
using Newtonsoft.Json;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;
using System.Windows.Media.Media3D;

namespace PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository
{
    public class CompraSrcRepository : ICompraSrcRepository
    {
        public IEnumerable<ProductDto> searchProducts(string nameProduct)
        {

            var parameters = new[]
            {
                new SqlParameter("@NombreProducto", nameProduct)
            };

            var data = DataBaseHelper.ExecuteStoredProcedure("spBuscarProductoCercano", parameters);

            var products = data.AsEnumerable().Select<DataRow, ProductDto>(row => Mapper.Map<DataRow, ProductDto>(row)).ToList();

            return products;

        }


        public void InsertProdCuencidencia(InsertProdCuencidenciaDto data)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdProductoErp", data.IdProductoErp),
                new SqlParameter("@NombreProdErp", data.NombreProdErp),
                new SqlParameter("@NombreProdSrc", data.NombreProdSrc),
                new SqlParameter("@RucEmpresa", data.RucEmpresa)
            };
            DataBaseHelper.ExecuteStoredProcedureWithParams("spInsertarOActualizarCoincidenciaProdSrc", parameters);
        }

        public IEnumerable<SucursalDto> getAllSucursal()
        {
            var result =  DataBaseHelper.ExecuteStoredProcedure("spObtenerPuntosVentaConAlmacen");

            var puntosVentaConAlmacen = result.AsEnumerable()
                .Select<DataRow, SucursalDto>(row => Mapper.Map<DataRow, SucursalDto>(row))
                .ToList();

            return puntosVentaConAlmacen;
        }

        public List<CoincidenciaProdSrcDto> ObtenerCoincidenciasProdSrcPorRuc(string rucEmpresa)
        {
            var parameters = new[]
            {

                new SqlParameter("@RucEmpresa", rucEmpresa)
             };

            var result = DataBaseHelper.ExecuteStoredProcedure("spObtenerCoincidenciasProdSrcPorRuc", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CoincidenciaProdSrcDto>(row => Mapper.Map<DataRow, CoincidenciaProdSrcDto>(row))
                .ToList();

            return coincidencias;
        }
        public List<CoincidenciaProdSrcDto> BuscarProductoPorNombreCuencidenciaSrc(string NombreProd, string rucEmpresa)
        {
            var parameters = new[]
            {
                new SqlParameter("@NombreProducto", NombreProd),
                new SqlParameter("@Ruc", rucEmpresa)
             };

            var result = DataBaseHelper.ExecuteStoredProcedure("spBuscarProductoPorNombreCuencidenciaSrc", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CoincidenciaProdSrcDto>(row => Mapper.Map<DataRow, CoincidenciaProdSrcDto>(row))
                .ToList();

            return coincidencias;
        }

        public ValidarImpoDto BuscarCompraPorSerieYNumero(string serie, string compra, string idRecepcion)
        {
            var parameters = new[]
            {

                new SqlParameter("@serie", serie),
                new SqlParameter("@numCompra", compra),
                new SqlParameter("@idRecepcion", idRecepcion)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("sp_BuscarCompraPorSerieYNumero", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, ValidarImpoDto>(row => Mapper.Map<DataRow, ValidarImpoDto>(row))
                .ToList();

            return coincidencias.FirstOrDefault() ?? new ValidarImpoDto();
        }

        public List<CliProveedorDto> GetCliProByRUCOrRazonComercial(string rucEmpresa, string razonSocial)
        {
            var parameters = new[]
            {
                new SqlParameter("@RUC", rucEmpresa)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("sp_GetCliProByRUCRazonComercial", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, CliProveedorDto>(row => Mapper.Map<DataRow, CliProveedorDto>(row))
                .ToList();

            return coincidencias;
        }


        public List<ClaseTipoSunatDto> GetClaseDocByTipoSunat(string tipoSunat)
        {
            var parameters = new[]
            {
                new SqlParameter("@TipoSunat", tipoSunat)
            };

            var result = DataBaseHelper.ExecuteStoredProcedure("sp_GetClaseDocByTipoSunat", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, ClaseTipoSunatDto>(row => Mapper.Map<DataRow, ClaseTipoSunatDto>(row))
                .ToList();

            return coincidencias;
        }

        public List<PeriodoDto> ObtenerPeriodosPorFecha(int anio, int mes)
        {
            var parameters = new[]
            {
                new SqlParameter("@Anio", anio),
                new SqlParameter("@Mes", mes)

            };

            var result = DataBaseHelper.ExecuteStoredProcedure("sp_ObtenerPeriodosPorFecha", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, PeriodoDto>(row => Mapper.Map<DataRow, PeriodoDto>(row))
                .ToList();

            return coincidencias;
        }

        public List<PlantillasDto> spListarEspecificasCompras()
        {

            var result =  DataBaseHelper.ExecuteStoredProcedure("spListarEspecificasCompras");

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, PlantillasDto>(row => Mapper.Map<DataRow, PlantillasDto>(row))
                .ToList();

            return coincidencias;
        }

        public List<TipoOperacionDto> sp_GetTipoOperacion(string codSunat)
        {

            var parameters = new[]
             {
                new SqlParameter("@codSunat", codSunat),

            };

            var result = DataBaseHelper.ExecuteStoredProcedure("sp_GetTipoOperacion", parameters);

            var coincidencias = result.AsEnumerable()
                .Select<DataRow, TipoOperacionDto>(row => Mapper.Map<DataRow, TipoOperacionDto>(row))
                .ToList();

            return coincidencias;
        }

        public void sp_InsertTemporalBitacoraSrc(TemporalBitacoraSrcDto data)
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

            DataBaseHelper.ExecuteStoredProcedureWithParams("sp_InsertTemporalBitacoraSrc", parameters);
        }

        public void UpdateConfiguracionInicial(int reiniciar)
        {
            var parameters = new[]
            {
                new SqlParameter("@reiniciar", reiniciar)
            };

            DataBaseHelper.ExecuteStoredProcedureWithParams("Sp_UpdateConfiguracionFormSrc", parameters);
        }

        public void InsertarDetalleTemporalSrc(DetalleTemporalBitacoraSrcDto data)
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

            DataBaseHelper.ExecuteStoredProcedure("sp_insertDetalleTemporalSrc", parameters);
        }


        public IEnumerable<ProductDto> BuscarProductoPorId(string idProducto)
        {
            var parameters = new[]
            {
        new SqlParameter("@IdProducto", idProducto)};

            var data = DataBaseHelper.ExecuteStoredProcedure("spBuscarProductoPorId", parameters);

            var products = data.AsEnumerable()
                .Select<DataRow, ProductDto>(row => Mapper.Map<DataRow, ProductDto>(row))
                .ToList();

            return products;
        }


        public void InsertarCliPro(ProveedorDto proveedor)
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

            DataBaseHelper.ExecuteStoredProcedureWithParams("sp_InsertCliPro", parameters);
        }

        public void ActualizarProductoCompraTemporalMonitoreoSRC(string idProducto, string numCompra, string serieCompra, string NomProducto, decimal api, decimal temp, string scop)
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
            };

            DataBaseHelper.ExecuteStoredProcedureWithParams("spActualizarCompraTemporalMonitoreoSRC", parameters);
        }
        public void ActualizaCabeceraTemporalMonitoreoSRC(string idRecepcion, int idSucursal, int IdPeriodo)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcion", idRecepcion),
                new SqlParameter("@idSucursal", idSucursal),
                new SqlParameter("@idPeriodo", IdPeriodo)
            };

            DataBaseHelper.ExecuteStoredProcedureWithParams("spActualizaCabeceraTemporalMonitoreoSRC", parameters);
        }
        public void InsertarEliminarComprobanteSrc(string idRecepcionSrc, string nCompra, int idPeriodo, DateTime fechaLlegada, string scop)
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

            DataBaseHelper.ExecuteStoredProcedureWithParams("spInsertarEliminarComprobanteSrc", parameters);
        }

        public List<CompraTemporalMonitoreoSrcDto> ObtenerCompraTemporalMonitoreoSrc(int? estado = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@estado", estado ?? (object)DBNull.Value)
            };

            var data = DataBaseHelper.ExecuteStoredProcedure("spObtenerCompraTemporalMonitoreoSRC", parameters);

            var compraMonitoreos = data.AsEnumerable()
                .Select<DataRow, CompraTemporalMonitoreoSrcDto>(row => Mapper.Map<DataRow, CompraTemporalMonitoreoSrcDto>(row))
                .ToList();

            return compraMonitoreos;
        }

        public ValidarCierreDto validarCompraCerrado(DateTime fecha, int idPuntoVenta)
        {
            var parameters = new[]
            {
                new SqlParameter("@fecha", fecha ),
                new SqlParameter("@idPuntoVenta", idPuntoVenta)
            };

            var data = DataBaseHelper.ExecuteStoredProcedure("sp_ConsultarSituacionCierreCompras", parameters);

            var compraMonitoreos = data.AsEnumerable()
                .Select<DataRow, ValidarCierreDto>(row => Mapper.Map<DataRow, ValidarCierreDto>(row))
                .ToList();

            if(compraMonitoreos.Count == 0)
            {
                return new ValidarCierreDto();
            }

            return compraMonitoreos[0];
        }




        public List<CompraTemporalMonitoreoSrcDto> ObtenerCompraMonitoreoTemporalPorIdRecepcion(string idRecepcion)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", idRecepcion)
            };

             var data =  DataBaseHelper.ExecuteStoredProcedure("sp_ObtenerCompraTemporalMonitoreo", parameters);

             var compraMonitoreos = data.AsEnumerable()
                .Select<DataRow, CompraTemporalMonitoreoSrcDto>(row => Mapper.Map<DataRow, CompraTemporalMonitoreoSrcDto>(row))
                .ToList();

            return compraMonitoreos;
        }

        public List<CompraTemporalMonitoreoSrcDto> ObtenerCompraMonitoreoSrcPorIdRecepcion(string idRecepcion)
        {
            var parameters = new[]
            {
                new SqlParameter("@idRecepcionSrc", idRecepcion)
            };

            var data = DataBaseHelper.ExecuteStoredProcedure("sp_ObtenerCompraTemporalMonitoreoSRC", parameters);

            var compraMonitoreos = data.AsEnumerable()
               .Select<DataRow, CompraTemporalMonitoreoSrcDto>(row => Mapper.Map<DataRow, CompraTemporalMonitoreoSrcDto>(row))
               .ToList();

            return compraMonitoreos;
        }

        public void InsertarCompraTemporalActualizar(CompraTemporalMonitoreoSrcDto data)
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
                new SqlParameter("@IdPeriodo", data.IdPeriodo),
                // new SqlParameter("@fiseSrc", data.fiseTotal),
                //new SqlParameter("@idAsientoTipo", data.IdPlantilla),
            };

            DataBaseHelper.ExecuteStoredProcedureWithParams("spInsertCompraTemporalSRC", parameters);


        }

        public void InsertarCompraTemporal(CompraDto compra, CompraDetalleDto dCompra)
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
                new SqlParameter("@IdPeriodo", compra.idPeriodo),
                new SqlParameter("@fiseSrc", compra.fiseTotal),
                new SqlParameter("@idAsientoTipo", compra.IdPlantilla),
            };

            DataBaseHelper.ExecuteStoredProcedureWithParams("spInsertCompraTemporal", parameters);


        }

        public GetConfiguracionDto GetConfiguracionInicial()
        {
          
            var data =  DataBaseHelper.ExecuteStoredProcedure("Sp_GetConfigurationSrc");

            var configuracion = data.AsEnumerable()
                .Select<DataRow, GetConfiguracionDto>(row => Mapper.Map<DataRow, GetConfiguracionDto>(row))
                .ToList();
            if(configuracion.Count <= 0)
            {
                return new GetConfiguracionDto();
            }
            return configuracion[0];
        }
        public GetProductExtDto getIdProductoExt(string idProducto)
        {
            var parameters = new[]
            {
                new SqlParameter("@idProducto", idProducto)
            };
            var data = DataBaseHelper.ExecuteStoredProcedure("Sp_GetIdProductoExt", parameters);

            var configuracion = data.AsEnumerable()
                .Select<DataRow, GetProductExtDto>(row => Mapper.Map<DataRow, GetProductExtDto>(row))
                .ToList();
            if (configuracion.Count <= 0)
            {
                return new GetProductExtDto();
            }
            return configuracion[0];
        }


        public void crearBackup(List<CompraDto> data)
        {
            var backup = JsonConvert.SerializeObject(data);

            var backupEncriptado = HFunciones.Codificar(backup);

            var parameters = new[]
            {
                new SqlParameter("@jsonNuevo", backupEncriptado)
            };
            DataBaseHelper.ExecuteStoredProcedureWithParams("sp_createJsonBackupSrc", parameters);
        }


        public List<CompraDto> getBackup()
        {
            var dt = DataBaseHelper.ExecuteStoredProcedure("sp_listarJsonBackupSrc");
            if (dt != null && dt.Rows.Count > 0)
            {
                string json = dt.Rows[0]["jsonRespaldado"].ToString();
                string backupDesencriptado = HFunciones.Codificar(json);
                return JsonConvert.DeserializeObject<List<CompraDto>>(backupDesencriptado);
            }
            else
            {
                return new List<CompraDto>();
            }
        }

        public List<PermisosInterfacesDto> sp_ObtenerPermisosPorInterfaceYUsuarioSrc(int idUsuario)
        {

            var parameters = new[]
             {
                new SqlParameter("@IdUsuario", idUsuario),

            };

            var result =  DataBaseHelper.ExecuteStoredProcedure("sp_ObtenerPermisosPorInterfaceYUsuarioSrc", parameters);

            var data = result.AsEnumerable()
                .Select<DataRow, PermisosInterfacesDto>(row => Mapper.Map<DataRow, PermisosInterfacesDto>(row))
                .ToList();

            return data;
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