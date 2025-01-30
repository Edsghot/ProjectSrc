using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
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

        public async Task InsertarDCompra(CompraDetalleDto dCompra)
        {
            var parameters = new[]
            {
        new SqlParameter("@nCompra", dCompra.nCompra ?? (object)DBNull.Value),
        new SqlParameter("@idProducto", dCompra.IdProducto ),
        new SqlParameter("@Cantidad", dCompra.Cantidad),
        new SqlParameter("@Bultos", dCompra.Bultos), // Valor por defecto
        new SqlParameter("@Precio", dCompra.PrecioUnitarioSinIgv),
        new SqlParameter("@idTipoIgv", 28), // Valor por defecto
        new SqlParameter("@pIgv", dCompra.Igv),
        new SqlParameter("@Flete", 1), // Valor por defecto
        new SqlParameter("@FechaVenc", new DateTime(1900, 1, 1)),
            // Se asegura que acepte NULL
        new SqlParameter("@Bonificacion", false), // Valor por defecto
        new SqlParameter("@CUR", Guid.NewGuid()), // Valor por defecto
        new SqlParameter("@PrecioBase", dCompra.PrecioUnitarioSinIgv),
        new SqlParameter("@Descuento1", 0), // Si Dscto es NULL, usa 0
        new SqlParameter("@Descuento2", 1), // Valor por defecto
        new SqlParameter("@Descuento3", 1), // Valor por defecto
        new SqlParameter("@Descuento4", 1), // Valor por defecto
        new SqlParameter("@API", decimal.TryParse(dCompra.Api, out decimal apiValue) ? (object)apiValue : 0.00m),
        new SqlParameter("@temperatura", (object)dCompra.Temp ?? DBNull.Value),
           new SqlParameter("@DT",  DateTime.Now ),

        new SqlParameter("@cantidadRecibidaGuia", 0), // Valor por defecto
        new SqlParameter("@pPercepcionCompTeso", 0), // Valor por defecto
        new SqlParameter("@esOrdenCompraGuiaCompra", false), // Valor por defecto
        new SqlParameter("@fise", (object)dCompra.fise ?? 0) // Si fise es NULL, usa 0
    };

            await DataBaseHelper.ExecuteStoredProcedureAsync("spInsertDCompraErp", parameters);
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
            try
            {
                compra.nOrdenCompra = null; // Asegurar que tiene un valor dentro del rango permitido
                compra.Condicion = "R";
               
                var parameters = new[]
                {
            new SqlParameter("@nCompra", compra.SerieCompra+compra.NumCompra),
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
            new SqlParameter("@idTransportista", (object)compra.idTransportista ?? DBNull.Value),
            new SqlParameter("@idPlaca", (object)compra.idPlaca ?? DBNull.Value),
            new SqlParameter("@idChofer", (object)compra.idChofer ?? DBNull.Value),
            new SqlParameter("@IdPlantilla", compra.IdPlantilla),
            new SqlParameter("@Obs", compra.Observacion),
            new SqlParameter("@SubTotal", compra.SubTotal),
            new SqlParameter("@Igv", compra.TotalIGV),
            new SqlParameter("@Total", compra.TotalPagar),
            new SqlParameter("@Importacion", compra.Importacion),
            new SqlParameter("@Automatica", compra.Automatica),
            new SqlParameter("@numConstanciaDep", (object)compra.numConstanciaDep ?? DBNull.Value),
            new SqlParameter("@fecConstanciaDep", (object)compra.fecConstanciaDep ?? DBNull.Value),
            new SqlParameter("@FechaLlegada", (object)compra.FechaLlegada ?? DBNull.Value),
            new SqlParameter("@IdTurno", (object)compra.IdTurno ?? DBNull.Value),
            new SqlParameter("@RelGuiaCompra", compra.RelGuiaCompra),
            new SqlParameter("@PrecioIncluyeIGV", compra.PrecioIncluyeIGV),
            new SqlParameter("@tipoFechaRegCompras", (object)compra.tipoFechaRegCompras ?? DBNull.Value),
            new SqlParameter("@fechaEspecialRC", (object)compra.fechaEspecialRC ?? DBNull.Value),
            new SqlParameter("@servicioIntangible", compra.servicioIntangible),
            new SqlParameter("@idTipoOperacion", (object)compra.idTipoOperacion ?? DBNull.Value),
            new SqlParameter("@idDepartamento", compra.idDepartamento),
            new SqlParameter("@nOrdenCompra", compra.nOrdenCompra),
            new SqlParameter("@detraccion", compra.detraccion),
            new SqlParameter("@tieneConsignaciones", compra.tieneConsignaciones),
            new SqlParameter("@fleteTotal", compra.fleteTotal),
            new SqlParameter("@distribuir", compra.distribuir),
            new SqlParameter("@idProcesoAsociado",  DBNull.Value),
            new SqlParameter("@nProcesoAsociado", (object)compra.nProcesoAsociado ?? DBNull.Value),
            new SqlParameter("@guiaRecibida", (object)compra.guiaRecibida ?? DBNull.Value),
            new SqlParameter("@nPercepcion", (object)compra.nPercepcion ?? DBNull.Value),
            new SqlParameter("@fechaPercepcion", (object)compra.fechaPercepcion ?? DBNull.Value),
            new SqlParameter("@percepcionTotal", compra.TotalPercepcion),
            new SqlParameter("@pRetencion", compra.pRetencion),
            new SqlParameter("@nCompraPlus", compra.nCompraPlus),
            new SqlParameter("@nOrdenCompraProveedor", (object)compra.nOrdenCompraProveedor ?? DBNull.Value),
            new SqlParameter("@fiseTotal", compra.fiseTotal),
            new SqlParameter("@idClasificacionBienesServicios", (object)compra.idClasificacionBienesServicios ?? DBNull.Value),
            new SqlParameter("@idTipoFacturacionGuiaRemision", compra.idTipoFacturacionGuiaRemision)
        };

                // 🔍 **Verificar si algún parámetro está excediendo la longitud permitida**
                foreach (var param in parameters)
                {
                    if (param.Value != DBNull.Value && param.Value is string strValue)
                    {
                        int maxLength = GetMaxLength(param.ParameterName);
                        if (maxLength > 0 && strValue.Length > maxLength)
                        {
                            throw new Exception($"⚠️ El parámetro {param.ParameterName} excede el límite de {maxLength} caracteres. Valor recibido: '{strValue}'");
                        }
                    }
                }

                // 📌 **Ejecutar el procedimiento almacenado**
                await DataBaseHelper.ExecuteStoredProcedureAsync("InsertCompraErp", parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception($"❌ Error ejecutando el procedimiento almacenado 'InsertCompraErp': {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Error en la inserción: {ex.Message}");
            }
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