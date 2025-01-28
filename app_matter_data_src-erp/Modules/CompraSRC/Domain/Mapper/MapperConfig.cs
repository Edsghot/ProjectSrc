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
            .Member(dest => dest.ProductId, src => src["ProductoId"] != DBNull.Value ? src["ProductoId"].ToString() : "")
            .Member(dest => dest.ProductName, src => src["NombreProducto"] != DBNull.Value ? src["NombreProducto"].ToString() : "");

        Mapper.Register<DataRow, SucursalDto>()
            .Member(dest => dest.IdPuntoVenta, src => src["IdPuntoVenta"] != DBNull.Value ? Convert.ToInt32(src["IdPuntoVenta"]) : 0)
            .Member(dest => dest.NomPuntoVenta, src => src["nomPuntoVenta"] != DBNull.Value ? src["nomPuntoVenta"].ToString() : "")
            .Member(dest => dest.LocalFisico, src => src["localFisico"] != DBNull.Value ? src["localFisico"].ToString() : "")
            .Member(dest => dest.SucursalSRC, src => src["SucursalSRC"] != DBNull.Value ? src["SucursalSRC"].ToString() : "")
            .Member(dest => dest.NomAlmacen, src => src["NomAlmacen"] != DBNull.Value ? src["NomAlmacen"].ToString() : "")
            .Member(dest => dest.IdAlmacen, src => src["IdAlmacen"] != DBNull.Value ? Convert.ToInt32(src["IdAlmacen"]) : 0)
            .Member(dest => dest.AlmacenSrc, src => src["AlmacenSrc"] != DBNull.Value ? src["AlmacenSrc"].ToString() : "");

        Mapper.Register<DataRow, CoincidenciaProdSrcDto>()
            .Member(dest => dest.IdCoincidencia, src => src["IdCoincidencia"] != DBNull.Value ? Convert.ToInt32(src["IdCoincidencia"]) : 0)
            .Member(dest => dest.IdProductoErp, src => src["IdProductoErp"] != DBNull.Value ? src["IdProductoErp"].ToString() : "")
            .Member(dest => dest.NombreProdErp, src => src["NombreProdErp"] != DBNull.Value ? src["NombreProdErp"].ToString() : "")
            .Member(dest => dest.NombreProdSrc, src => src["NombreProdSrc"] != DBNull.Value ? src["NombreProdSrc"].ToString() : "")
            .Member(dest => dest.RucEmpresa, src => src["RucEmpresa"] != DBNull.Value ? src["RucEmpresa"].ToString() : "")
            .Member(dest => dest.Validado, src => src["Validado"] != DBNull.Value ? Convert.ToBoolean(src["Validado"]) : false)
            .Member(dest => dest.FechaValidacion, src => src["FechaValidacion"] != DBNull.Value ? Convert.ToDateTime(src["FechaValidacion"]) : DateTime.MinValue);

        Mapper.Register<DataRow, CliProveedorDto>()
            .Member(dest => dest.IdCliPro, src => src["idCliPro"] != DBNull.Value ? src["idCliPro"].ToString() : "");

        Mapper.Register<DataRow, ClaseTipoSunatDto>()
            .Member(dest => dest.IdClaseDoc, src => src["IdClaseDoc"] != DBNull.Value ? src["IdClaseDoc"].ToString() : "");

        Mapper.Register<DataRow, PeriodoDto>()
            .Member(dest => dest.IdPeriodo, src => src["IdPeriodo"] != DBNull.Value ? Convert.ToInt32(src["IdPeriodo"]) : 0)
            .Member(dest => dest.NomPeriodo, src => src["NomPeriodo"] != DBNull.Value ? src["NomPeriodo"].ToString() : "")
            .Member(dest => dest.IdTipoPeriodo, src => src["IdTipoPeriodo"] != DBNull.Value ? Convert.ToInt32(src["IdTipoPeriodo"]) : 0)
            .Member(dest => dest.FechaI, src => src["FechaI"] != DBNull.Value ? Convert.ToDateTime(src["FechaI"]) : DateTime.MinValue)
            .Member(dest => dest.FechaFin, src => src["FechaF"] != DBNull.Value ? Convert.ToDateTime(src["FechaF"]) : DateTime.MinValue)
            .Member(dest => dest.Cerrado, src => src["Cerrado"] != DBNull.Value ? Convert.ToBoolean(src["Cerrado"]) : false);

        Mapper.Register<DataRow, PlantillasDto>()
            .Member(dest => dest.IdPlantilla, src => src["idPlantilla"] != DBNull.Value ? src["idPlantilla"].ToString() : "")
            .Member(dest => dest.NomPlantilla, src => src["nomPlantilla"] != DBNull.Value ? src["nomPlantilla"].ToString() : "")
            .Member(dest => dest.CompraSrc, src => src["CompraSrc"] != DBNull.Value ? Convert.ToBoolean(src["CompraSrc"]) : false);

        Mapper.Register<DataRow, TipoOperacionDto>()
            .Member(dest => dest.IdTipoOperacion, src => src["idTipoOperacion"] != DBNull.Value ? Convert.ToInt32(src["idTipoOperacion"]) : 0)
            .Member(dest => dest.NomTipoOperacion, src => src["nomTipoOperacion"] != DBNull.Value ? src["nomTipoOperacion"].ToString() : "");
    }


}