using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
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
        Task InsertarCompraAsync(CompraDto compra);
        Task InsertarDCompra(CompraDetalleDto dCompra);
        Task InsertarCompraTemporal(CompraDto compra,CompraDetalleDto data);
    }
}