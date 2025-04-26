using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.general;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Static;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcInputPort
    {
        Task<List<CompraDto>> ObtenerDataSrc();
        Task<string> GetIdRecepcion(string documentoProveedor, string codigo);
        Task<MenuDto> GetMenu();
        Task<bool> InsertCompra(int mes, int anio, string IdRecepcion);
        Task EscanearDCompra(string IdProduct, string NombreProducto);
        Task<bool> validarImportacion(string serie, string numCompra,string idRecepcion);
        //nuevos 
        GenericErrorsDto GetErrorsDetail(string idRecepcion);
        Task<bool> ProductsValidated(string idRecepcion);
        Task ActualizarSucursal(string idRecepcion, string IdPuntoVenta, string nomPuntoVenta);
        Task ActualizarScopApiTemp(string idRecepcion, string nomProducto, string scop, decimal api, decimal temp);
        Task updateConfiguration(int reiniciar);
        Task<List<CompraDto>> obtenerDataDelSrc();
        void createBackup();
        CompraDto ObtenerCompraPorIdRecepcion(string idRecepcion);
        CompraDto ObtenerCompraPorDocumentoProveedor(string documentoProveedor, string codigo);
    }
}
