using PknoPlusCS.Global.ApiClient;
using PknoPlusCS.Modules.CompraSRC.Application.Port;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Sucursal;
using PknoPlusCS.Modules.CompraSRC.Domain.Dto.Validacion;
using PknoPlusCS.Modules.CompraSRC.Domain.IRepository;
using PknoPlusCS.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PknoPlusCS.Modules.CompraSRC.Application.Adapter
{
    public class CompraSrcImportadosAdapter : ICompraSrcImportadosInputPort
    {

        private readonly IApiClient apiClient;
        private readonly ICompraSrcRepository compraSrcRepository;

        public CompraSrcImportadosAdapter()
        {
            this.apiClient = new ApiClient();
            this.compraSrcRepository = new CompraSrcRepository();
        }

        public async Task<List<CompraTemporalMonitoreoSrcDto>> ListarImportados(int estatus)
        {

            var data =  compraSrcRepository.ObtenerCompraTemporalMonitoreoSrc(estatus);
            var sucursal = (  compraSrcRepository.getAllSucursal()).ToList();

            DatosImportadosStatic.Data = data;
            DatosImportadosStatic.Sucursales = sucursal;
            return data;
        }

     

        public async Task<string> GetIdRecepcion(string codigo, string ruc)
        {
            var arreglo = codigo.Split('-');

            var data = DatosImportadosStatic.Data.FirstOrDefault(x => x.SerieCompra == arreglo[0] && x.NumCompra == arreglo[1] && x.RucPersona == ruc);

            return data.IdRecepcionSrc;
        }

        public async Task<CompraTemporalMonitoreoSrcDto> GetAllByIdRecepcion(string idRecepcion)
        {
            var data = DatosImportadosStatic.Data.FirstOrDefault(x => x.IdRecepcionSrc == idRecepcion);

            return data;
        }

        public List<SucursalDto> GetAllSucursales()
        {
            var data = DatosImportadosStatic.Sucursales.ToList(); 

            var sucursalTodos = data.FirstOrDefault(s => s.NomPuntoVenta == "Todos");

            if (sucursalTodos == null)
            {
                sucursalTodos = new SucursalDto
                {
                    IdPuntoVenta = "-1", 
                    NomPuntoVenta = "Todos",
                    SucursalSRC = "False",
                    AlmacenSrc = "False"
                };
                data.Add(sucursalTodos);
            }

            data = data.OrderBy(s => s.NomPuntoVenta == "Todos" ? 0 : 1).ToList();

            return data;
        }



        public string GetCodigoSucursal(string nameSucursal)
        {
            var data = DatosImportadosStatic.Sucursales.FirstOrDefault(x => x.NomPuntoVenta == nameSucursal);

            return data.IdPuntoVenta;
        }


        public async Task<List<CompraTemporalMonitoreoSrcDto>> GetComprasPorIdRecepcion(string idRecepcion)
        {
            var data =  compraSrcRepository.ObtenerCompraMonitoreoSrcPorIdRecepcion(idRecepcion);
            return data;
        }

        public async Task<DateTime> GetPeriodo(string idRecepcion)
        {
            var data =  compraSrcRepository.ObtenerCompraMonitoreoTemporalPorIdRecepcion(idRecepcion);

            return data[0].FechaPeriodo;
        }

        public async Task InsertarDelTemporalActualizar(string idRecepcion)
        {
            var dataa = await GetComprasPorIdRecepcion(idRecepcion);

            foreach (var item in dataa) {
                try
                {
                     compraSrcRepository.InsertarCompraTemporalActualizar(item);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en InsertarDelTemporalActualizar: {ex.Message}");
                }
            }
        }

        public void updateConfiguration(int reiniciar)
        {
              compraSrcRepository.UpdateConfiguracionInicial(reiniciar);
        }

        public ValidarCierreDto validarCierreArea(DateTime fecha, int idPuntoVenta)
        {
           return compraSrcRepository.validarCompraCerrado(fecha, idPuntoVenta);
           
        }
    }
}
