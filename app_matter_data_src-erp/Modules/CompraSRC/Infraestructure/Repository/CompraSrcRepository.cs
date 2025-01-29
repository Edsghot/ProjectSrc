using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using app_matter_data_src_erp.Global.DataBase;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
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

        public async Task<List<CliProveedorDto>> GetCliProByRUCOrRazonComercial(string rucEmpresa, string razonSocial)
        {
            var parameters = new[]
            {
                new SqlParameter("@RUC", rucEmpresa),
                new SqlParameter("@RazonComercial", razonSocial)
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

        public async Task<IEnumerable<ProductDto>> BuscarProductoPorId(string idProducto)
        {
            var parameters = new[]
            {
        new SqlParameter("@IdProducto", idProducto)
    };

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
                new SqlParameter("@Tipo", 'C'),
                new SqlParameter("@RazonComercial", proveedor.RazonSocial),
                new SqlParameter("@IdClaseDocDef", 1), // Valor por defecto
                new SqlParameter("@RUC", proveedor.NumeroDocumento),
                new SqlParameter("@IdTipoDI", "RUC"), // Valor por defecto
                new SqlParameter("@DNI", string.Empty), // Valor por defecto
                new SqlParameter("@Sexo", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdDepa", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdProv", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdDistrito", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdPuntoVenta", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdRuta", DBNull.Value), // Valor por defecto
                new SqlParameter("@IdGiroNeg", 1), // Valor por defecto
                new SqlParameter("@IdTipoNeg", DBNull.Value), // Valor por defecto
                new SqlParameter("@NroDiasCredito", DBNull.Value), // Valor por defecto
                new SqlParameter("@CreditoMN", DBNull.Value), // Valor por defecto
                new SqlParameter("@CreditoME", DBNull.Value), // Valor por defecto
                new SqlParameter("@Clasificacion","$"), // Valor por defecto
                new SqlParameter("@CreditoCant", DBNull.Value), // Valor por defecto
                new SqlParameter("@lineaCredito", 1), // Valor por defecto
                new SqlParameter("@idSubCondicion", DBNull.Value) // Valor por defecto
            };

            await DataBaseHelper.ExecuteStoredProcedureAsync("InsertCliPro1", parameters);
        }

        public async Task InsertarCompraAsync(CompraDto compra)
        {
            compra.nOrdenCompra = compra.NumCompra;
            var parameters = new[]
            {
                new SqlParameter("@nCompra", compra.NumCompra),
                new SqlParameter("@idCliPro", compra.idCliPro),
                new SqlParameter("@idClaseDoc", compra.idClaseDoc),
                new SqlParameter("@idAlmacen", compra.IdAlmacen),
                new SqlParameter("@FechaDig", compra.FechaDig),
                new SqlParameter("@Fecha", compra.FechaEmision),
                new SqlParameter("@FechaOperativa", compra.FechaOperativa),
                new SqlParameter("@FechaVenc", compra.FechaVencimiento),
                new SqlParameter("@idPeriodo", compra.idPeriodo),
                new SqlParameter("@Moneda", compra.Moneda),
                new SqlParameter("@TipoCambio", compra.TipoCambio),
                new SqlParameter("@Condicion", compra.Condicion),
                new SqlParameter("@NGuiaRemision", compra.NGuiaRemision),
                new SqlParameter("@idTransportista", compra.idTransportista),
                new SqlParameter("@idPlaca", compra.idPlaca),
                new SqlParameter("@idChofer", compra.idChofer),
                new SqlParameter("@IdPlantilla", compra.IdPlantilla),
                new SqlParameter("@Obs", compra.Observacion),
                new SqlParameter("@SubTotal", compra.SubTotal),
                new SqlParameter("@Igv", compra.TotalIGV),
                new SqlParameter("@Total", compra.TotalPagar),
                new SqlParameter("@Importacion", compra.Importacion),
                new SqlParameter("@Automatica", compra.Automatica),
                new SqlParameter("@numConstanciaDep", (object)compra.numConstanciaDep ?? DBNull.Value),
                new SqlParameter("@fecConstanciaDep", (object)compra.fecConstanciaDep ?? DBNull.Value),
                new SqlParameter("@FechaLlegada", compra.FechaLlegada),
                new SqlParameter("@IdTurno", compra.IdTurno),
                new SqlParameter("@RelGuiaCompra", compra.RelGuiaCompra),
                new SqlParameter("@PrecioIncluyeIGV", compra.PrecioIncluyeIGV),
                new SqlParameter("@tipoFechaRegCompras", compra.tipoFechaRegCompras),
                new SqlParameter("@fechaEspecialRC", (object)compra.fechaEspecialRC ?? DBNull.Value),
                new SqlParameter("@servicioIntangible", compra.servicioIntangible),
                new SqlParameter("@idTipoOperacion", compra.idTipoOperacion),
                new SqlParameter("@idDepartamento", compra.idDepartamento),
                new SqlParameter("@nOrdenCompra", compra.nOrdenCompra),
                new SqlParameter("@detraccion", compra.detraccion),
                new SqlParameter("@tieneConsignaciones", compra.tieneConsignaciones),
                new SqlParameter("@fleteTotal", compra.fleteTotal),
                new SqlParameter("@distribuir", compra.distribuir),
                new SqlParameter("@idProcesoAsociado", compra.idProcesoAsociado),
                new SqlParameter("@nProcesoAsociado", compra.nProcesoAsociado),
                new SqlParameter("@guiaRecibida", compra.guiaRecibida),
                new SqlParameter("@nPercepcion", compra.nPercepcion),
                new SqlParameter("@fechaPercepcion", (object)compra.fechaPercepcion ?? DBNull.Value),
                new SqlParameter("@percepcionTotal", compra.TotalPercepcion),
                new SqlParameter("@pRetencion", compra.pRetencion),
                new SqlParameter("@nCompraPlus", compra.nCompraPlus),
                new SqlParameter("@nOrdenCompraProveedor", compra.nOrdenCompraProveedor),
                new SqlParameter("@fiseTotal", compra.fiseTotal),
                new SqlParameter("@idClasificacionBienesServicios", compra.idClasificacionBienesServicios),
                new SqlParameter("@idTipoFacturacionGuiaRemision", compra.idTipoFacturacionGuiaRemision)
             };

            await DataBaseHelper.ExecuteStoredProcedureAsync("InsertCompraErp", parameters);
        }


    }
}