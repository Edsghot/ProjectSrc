using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using ExpressMapper;
using System;
using System.Data;

public static class MapperConfig
{
    public static void RegisterMappings()
    {
        Mapper.Register<DataRow, ProductDto>()
            .Member(dest => dest.ProductId, src => src["ProductoId"].ToString())
               .Member(dest => dest.ProductName, src => src["NombreProducto"].ToString());

        // Add more mappings as needed

        Mapper.Register<DataRow, SucursalDto>()
                .Member(dest => dest.IdPuntoVenta, src => Convert.ToInt32(src["IdPuntoVenta"]))
                .Member(dest => dest.NomPuntoVenta, src => src["nomPuntoVenta"].ToString())
                .Member(dest => dest.LocalFisico, src => src["localFisico"].ToString())
                .Member(dest => dest.SucursalSRC, src => src["SucursalSRC"].ToString())
                .Member(dest => dest.NomAlmacen, src => src["NomAlmacen"].ToString())
                .Member(dest => dest.IdAlmacen, src => Convert.ToInt32(src["IdAlmacen"]))
                .Member(dest => dest.AlmacenSrc, src => src["AlmacenSrc"].ToString());

        Mapper.Register<DataRow, CoincidenciaProdSrcDto>()
              .Member(dest => dest.IdCoincidencia, src => Convert.ToInt32(src["IdCoincidencia"]))
              .Member(dest => dest.IdProductoErp, src => src["IdProductoErp"].ToString())
              .Member(dest => dest.NombreProdErp, src => src["NombreProdErp"].ToString())
              .Member(dest => dest.NombreProdSrc, src => src["NombreProdSrc"].ToString())
              .Member(dest => dest.RucEmpresa, src => src["RucEmpresa"].ToString())
              .Member(dest => dest.Validado, src => Convert.ToBoolean(src["Validado"]))
              .Member(dest => dest.FechaValidacion, src => Convert.ToDateTime(src["FechaValidacion"]));

    }

}