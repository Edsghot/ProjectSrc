using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.bitacora;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Configuracion;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Permisos;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Proveedor;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;

namespace PknoPlusCS.Modules.CompraSRC.Domain.IRepository
{
    public interface ICompraSrcRepository
    {
        IEnumerable<ProductDto> searchProducts(string nameProduct);
        void InsertProdCuencidencia(InsertProdCuencidenciaDto data);
        IEnumerable<SucursalDto> getAllSucursal();
        List<CoincidenciaProdSrcDto> ObtenerCoincidenciasProdSrcPorRuc(string rucEmpresa);
        IEnumerable<ProductDto> BuscarProductoPorId(string idProducto);
        IEnumerable<ProductDto> BuscarProductoPorTipoAuxiliar(string idProducto);
        List<CliProveedorDto> GetCliProByRUCOrRazonComercial(string rucEmpresa, string razonSocial);
        List<ClaseTipoSunatDto> GetClaseDocByTipoSunat(string tipoSunat);
        List<PeriodoDto> ObtenerPeriodosPorFecha(int anio, int mes);
        List<PlantillasDto> spListarEspecificasCompras();
        List<TipoOperacionDto> sp_GetTipoOperacion(string codSunat);
        void InsertarCliPro(ProveedorDto proveedor);
        void InsertarCompraTemporal(CompraDto compra,CompraDetalleDto data);
        ValidarImpoDto BuscarCompraPorSerieYNumero(string serie, string compra,string idRecepcion);
        List<CompraTemporalMonitoreoSrcDto> ObtenerCompraTemporalMonitoreoSrc(int? estado = null);
        List<CoincidenciaProdSrcDto> BuscarProductoPorNombreCuencidenciaSrc(string NombreProd, string rucEmpresa);
        List<CompraTemporalMonitoreoSrcDto> ObtenerCompraMonitoreoTemporalPorIdRecepcion(string idRecepcion);
        void ActualizarProductoCompraTemporalMonitoreoSRC(string idProducto, string numCompra, string serieCompra, string NomProducto, decimal api, decimal temp,string scop);
        void InsertarEliminarComprobanteSrc(string idRecepcionSrc, string nCompra, int idPeriodo, DateTime fechaLlegada, string scop);
        void InsertarDetalleTemporalSrc(DetalleTemporalBitacoraSrcDto data);
        void sp_InsertTemporalBitacoraSrc(TemporalBitacoraSrcDto data);
        void ActualizaCabeceraTemporalMonitoreoSRC(string idRecepcion, int idSucursal, int IdPeriodo);
        void InsertarCompraTemporalActualizar(CompraTemporalMonitoreoSrcDto data);
        GetConfiguracionDto GetConfiguracionInicial();
        void UpdateConfiguracionInicial(int reiniciar);
        GetProductExtDto getIdProductoExt(string idProducto);
        List<CompraDto> getBackup();
        void crearBackup(List<CompraDto> data);
        List<CompraTemporalMonitoreoSrcDto> ObtenerCompraMonitoreoSrcPorIdRecepcion(string idRecepcion);
        List<PermisosInterfacesDto> sp_ObtenerPermisosPorInterfaceYUsuarioSrc(int idUsuario);
        ValidarCierreDto validarCompraCerrado(DateTime fecha, int idPuntoVenta);
    }
}