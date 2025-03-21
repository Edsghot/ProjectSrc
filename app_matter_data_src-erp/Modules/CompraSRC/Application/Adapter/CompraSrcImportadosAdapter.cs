﻿using app_matter_data_src_erp.Global.ApiClient;
using app_matter_data_src_erp.Modules.CompraSRC.Application.Port;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
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
            var sucursal = (await  compraSrcRepository.getAllSucursal()).ToList();

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
            var data = DatosImportadosStatic.Sucursales.ToList(); // Clonamos la lista

            // Buscar si ya existe un elemento con "Todos"
            var sucursalTodos = data.FirstOrDefault(s => s.NomPuntoVenta == "Todos");

            // Si no existe, lo agregamos al final
            if (sucursalTodos == null)
            {
                sucursalTodos = new SucursalDto
                {
                    IdPuntoVenta = "-1", // Valor especial para diferenciarlo
                    NomPuntoVenta = "Todos",
                    SucursalSRC = "False",
                    AlmacenSrc = "False"
                };
                data.Add(sucursalTodos);
            }

            // Reordenar la lista para que "Todos" quede al inicio
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

        public async Task updateConfiguration(int reiniciar)
        {
             await compraSrcRepository.UpdateConfiguracionInicial(reiniciar);
        }
    }
}
