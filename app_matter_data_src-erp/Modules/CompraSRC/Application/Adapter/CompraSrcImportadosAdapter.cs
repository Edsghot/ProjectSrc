using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.IRepository;
using app_matter_data_src_erp.Modules.CompraSRC.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Adapter
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

            var data = await compraSrcRepository.ObtenerCompraTemporalMonitoreoSrc(estatus);

            DatosImportadosStatic.Data = data;
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

        public async Task<List<CompraTemporalMonitoreoSrcDto>> GetComprasPorIdRecepcion(string idRecepcion)
        {
            var data = await compraSrcRepository.ObtenerCompraMonitoreoTemporalPorIdRecepcion(idRecepcion);
            return data;
        }

        public async Task<DateTime> GetPeriodo(string idRecepcion)
        {
            var data = await compraSrcRepository.ObtenerCompraMonitoreoTemporalPorIdRecepcion(idRecepcion);

            return data[0].FechaPeriodo;
        }

        public async Task InsertarDelTemporalActualizar(string idRecepcion)
        {
            var data = await GetComprasPorIdRecepcion(idRecepcion);

            foreach (var item in data) {
                
                 await compraSrcRepository.InsertarCompraTemporalActualizar(item);
            }
        }
    }
}
