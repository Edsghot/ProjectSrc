using PknoPlusCS.Global.ApiClient;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcImportadosInputPort
    {
        Task<List<CompraTemporalMonitoreoSrcDto>> ListarImportados(int estatus);
        Task<CompraTemporalMonitoreoSrcDto> GetAllByIdRecepcion(string idRecepcion);
        Task<string> GetIdRecepcion(string codigo, string ruc);
        Task<List<CompraTemporalMonitoreoSrcDto>> GetComprasPorIdRecepcion(string idRecepcion);
        Task InsertarDelTemporalActualizar(string idRecepcion);
        Task<DateTime> GetPeriodo(string idRecepcion);
        Task updateConfiguration(int reiniciar);
        List<SucursalDto> GetAllSucursales();
        string GetCodigoSucursal(string nameSucursal);
        ValidarCierreDto validarCierreArea(DateTime fecha, int idPuntoVenta);
    }
}
