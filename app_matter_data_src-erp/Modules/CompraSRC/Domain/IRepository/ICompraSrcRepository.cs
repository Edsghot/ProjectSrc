using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.bitacora;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Configuracion;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Proveedor;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;

namespace app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository
{
    public interface ICompraSrcRepository
    {
        Task<IEnumerable<ProductDto>> searchProducts(string nameProduct);
        Task InsertProdCuencidencia(InsertProdCuencidenciaDto data);
        Task<IEnumerable<SucursalDto>> getAllSucursal();
        Task<List<CoincidenciaProdSrcDto>> ObtenerCoincidenciasProdSrcPorRuc(string rucEmpresa);
        Task ActualizarPuntoVentaYAlmacen(int idPuntoVenta, int idAlmacen);
        Task<IEnumerable<ProductDto>> BuscarProductoPorId(string idProducto);
        Task<List<CliProveedorDto>> GetCliProByRUCOrRazonComercial(string rucEmpresa, string razonSocial);
        Task<List<ClaseTipoSunatDto>> GetClaseDocByTipoSunat(string tipoSunat);
        Task<List<PeriodoDto>> ObtenerPeriodosPorFecha(int anio, int mes);
        Task<List<PlantillasDto>> spListarEspecificasCompras();
        Task<List<TipoOperacionDto>> sp_GetTipoOperacion(string codSunat);
        Task InsertarCliPro(ProveedorDto proveedor);
        Task InsertarCompraTemporal(CompraDto compra,CompraDetalleDto data);
        Task<ValidarImpoDto> BuscarCompraPorSerieYNumero(string serie, string compra,string idRecepcion);
        Task<List<CompraTemporalMonitoreoSrcDto>> ObtenerCompraTemporalMonitoreoSrc(int? estado = null);
        Task<List<CoincidenciaProdSrcDto>> BuscarProductoPorNombreCuencidenciaSrc(string NombreProd, string rucEmpresa);
        Task<List<CompraTemporalMonitoreoSrcDto>> ObtenerCompraMonitoreoTemporalPorIdRecepcion(string idRecepcion);
        Task ActualizarProductoCompraTemporalMonitoreoSRC(string idProducto, string numCompra, string serieCompra, string NomProducto, decimal api, decimal temp,string scop);
        Task InsertarEliminarComprobanteSrc(string idRecepcionSrc, string nCompra, int idPeriodo, DateTime fechaLlegada, string scop);
        Task InsertarDetalleTemporalSrc(DetalleTemporalBitacoraSrcDto data);
        Task sp_InsertTemporalBitacoraSrc(TemporalBitacoraSrcDto data);
        Task ActualizaCabeceraTemporalMonitoreoSRC(string idRecepcion, int idSucursal, int IdPeriodo);
        Task InsertarCompraTemporalActualizar(CompraTemporalMonitoreoSrcDto data);
        Task<GetConfiguracionDto> GetConfiguracionInicial();
        Task UpdateConfiguracionInicial(int reiniciar);
        Task<GetProductExtDto> getIdProductoExt(string idProducto);
    }
}