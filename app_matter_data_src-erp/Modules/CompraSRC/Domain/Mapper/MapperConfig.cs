using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Configuracion;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.RepoDto;
using app_matter_data_src_erp.Modules.CompraSRC.Domain.Dto.Sucursal;
using ExpressMapper;
using System;
using System.Data;

public static class MapperConfig
{
    public static void RegisterMappings()
    {
        Mapper.Register<DataRow, GetConfiguracionDto>()
             .Member(dest => dest.reiniciar, src => src["reiniciar"] != DBNull.Value ? Convert.ToInt32(src["reiniciar"]) : 0);

        Mapper.Register<DataRow, ProductDto>()
            .Member(dest => dest.ProductId, src => src["ProductoId"] != DBNull.Value ? src["ProductoId"].ToString() : "")
            .Member(dest => dest.ProductName, src => src["NombreProducto"] != DBNull.Value ? src["NombreProducto"].ToString() : "");

        Mapper.Register<DataRow, ValidarImpoDto>()
           .Member(dest => dest.Resultado, src => src["Resultado"] != DBNull.Value ? Convert.ToInt32(src["Resultado"]) : 0);

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

        Mapper.Register<DataRow, GetProductExtDto>()
           .Member(dest => dest.IdProductoExt, src => src["idProductoExt"] != DBNull.Value ? src["idProductoExt"].ToString() : "")
           .Member(dest => dest.Combustible, src => src["Combustible"] != DBNull.Value ? Convert.ToBoolean(src["Combustible"]) : false);
        

        Mapper.Register<DataRow, TipoOperacionDto>()
            .Member(dest => dest.IdTipoOperacion, src => src["idTipoOperacion"] != DBNull.Value ? Convert.ToInt32(src["idTipoOperacion"]) : 0)
            .Member(dest => dest.NomTipoOperacion, src => src["nomTipoOperacion"] != DBNull.Value ? src["nomTipoOperacion"].ToString() : "");

        Mapper.Register<DataRow, CompraTemporalMonitoreoSrcDto>()
           .Member(dest => dest.Id, src => src["id"] != DBNull.Value ? Convert.ToInt32(src["id"]) : 0)
           .Member(dest => dest.IdComputadora, src => src["idComputadora"] != DBNull.Value ? Convert.ToInt32(src["idComputadora"]) : (int?)null)
           .Member(dest => dest.TipoDoc, src => src["tipoDoc"] != DBNull.Value ? src["tipoDoc"].ToString() : null)
           .Member(dest => dest.SerieCompra, src => src["serieCompra"] != DBNull.Value ? src["serieCompra"].ToString() : null)
           .Member(dest => dest.NumCompra, src => src["numCompra"] != DBNull.Value ? src["numCompra"].ToString() : null)
           .Member(dest => dest.RucPersona, src => src["rucPersona"] != DBNull.Value ? src["rucPersona"].ToString() : null)
           .Member(dest => dest.NomPersona, src => src["nomPersona"] != DBNull.Value ? src["nomPersona"].ToString() : null)
           .Member(dest => dest.Sucursal, src => src["sucursal"] != DBNull.Value ? src["sucursal"].ToString() : null)
           .Member(dest => dest.Fecha, src => src["fecha"] != DBNull.Value ? Convert.ToDateTime(src["fecha"]) : (DateTime?)null)
           .Member(dest => dest.FechaVenc, src => src["fechaVenc"] != DBNull.Value ? Convert.ToDateTime(src["fechaVenc"]) : (DateTime?)null)
           .Member(dest => dest.Moneda, src => src["moneda"] != DBNull.Value ? src["moneda"].ToString() : null)
           .Member(dest => dest.TcCompra, src => src["tcCompra"] != DBNull.Value ? Convert.ToDecimal(src["tcCompra"]) : (decimal?)null)
           .Member(dest => dest.Condicion, src => src["condicion"] != DBNull.Value ? src["condicion"].ToString() : null)
           .Member(dest => dest.NomTransportista, src => src["nomTransportista"] != DBNull.Value ? src["nomTransportista"].ToString() : null)
           .Member(dest => dest.RucTransportista, src => src["rucTransportista"] != DBNull.Value ? src["rucTransportista"].ToString() : null)
           .Member(dest => dest.DirTransportista, src => src["dirTransportista"] != DBNull.Value ? src["dirTransportista"].ToString() : null)
           .Member(dest => dest.PlacaTransportista, src => src["placaTransportista"] != DBNull.Value ? src["placaTransportista"].ToString() : null)
           .Member(dest => dest.MarcaTransportista, src => src["marcaTransportista"] != DBNull.Value ? src["marcaTransportista"].ToString() : null)
           .Member(dest => dest.CertInscripcion, src => src["certInscripcion"] != DBNull.Value ? src["certInscripcion"].ToString() : null)
           .Member(dest => dest.ConfiguracionVeh, src => src["configuracionVeh"] != DBNull.Value ? src["configuracionVeh"].ToString() : null)
           .Member(dest => dest.Cubicacion, src => src["cubicacion"] != DBNull.Value ? src["cubicacion"].ToString() : null)
           .Member(dest => dest.NomChofer, src => src["nomChofer"] != DBNull.Value ? src["nomChofer"].ToString() : null)
           .Member(dest => dest.BreveteChofer, src => src["breveteChofer"] != DBNull.Value ? src["breveteChofer"].ToString() : null)
           .Member(dest => dest.DestinoRC, src => src["destinoRC"] != DBNull.Value ? Convert.ToInt32(src["destinoRC"]) : (int?)null)
           .Member(dest => dest.Obs, src => src["obs"] != DBNull.Value ? src["obs"].ToString() : null)
           .Member(dest => dest.SubTotal, src => src["subTotal"] != DBNull.Value ? Convert.ToDecimal(src["subTotal"]) : (decimal?)null)
           .Member(dest => dest.Igv, src => src["igv"] != DBNull.Value ? Convert.ToDecimal(src["igv"]) : (decimal?)null)
           .Member(dest => dest.Total, src => src["total"] != DBNull.Value ? Convert.ToDecimal(src["total"]) : (decimal?)null)
           .Member(dest => dest.NDetraccion, src => src["nDetraccion"] != DBNull.Value ? src["nDetraccion"].ToString() : null)
           .Member(dest => dest.FDetraccion, src => src["fDetraccion"] != DBNull.Value ? Convert.ToDateTime(src["fDetraccion"]) : (DateTime?)null)
           .Member(dest => dest.FechaLlegada, src => src["fechaLlegada"] != DBNull.Value ? Convert.ToDateTime(src["fechaLlegada"]) : (DateTime?)null)
           .Member(dest => dest.PrecioIncluyeIGV, src => src["precioIncluyeIGV"] != DBNull.Value ? Convert.ToBoolean(src["precioIncluyeIGV"]) : (bool?)null)
           .Member(dest => dest.TipoOperacion, src => src["tipoOperacion"] != DBNull.Value ? Convert.ToInt32(src["tipoOperacion"]) : (int?)null)
           .Member(dest => dest.CentroCostos, src => src["centroCostos"] != DBNull.Value ? Convert.ToInt32(src["centroCostos"]) : (int?)null)
           .Member(dest => dest.SeriePer, src => src["seriePer"] != DBNull.Value ? src["seriePer"].ToString() : null)
           .Member(dest => dest.NumPer, src => src["numPer"] != DBNull.Value ? src["numPer"].ToString() : null)
           .Member(dest => dest.FechaPercepcion, src => src["fechaPercepcion"] != DBNull.Value ? Convert.ToDateTime(src["fechaPercepcion"]) : (DateTime?)null)
           .Member(dest => dest.PerTotal, src => src["perTotal"] != DBNull.Value ? Convert.ToDecimal(src["perTotal"]) : (decimal?)null)
           .Member(dest => dest.PRetencion, src => src["pRetencion"] != DBNull.Value ? Convert.ToDecimal(src["pRetencion"]) : (decimal?)null)
           .Member(dest => dest.ValidarTotales, src => src["validarTotales"] != DBNull.Value ? Convert.ToBoolean(src["validarTotales"]) : (bool?)null)
           .Member(dest => dest.IdProductoExt, src => src["idProductoExt"] != DBNull.Value ? src["idProductoExt"].ToString() : null)
           .Member(dest => dest.Cantidad, src => src["cantidad"] != DBNull.Value ? Convert.ToDecimal(src["cantidad"]) : (decimal?)null)
           .Member(dest => dest.Precio, src => src["precio"] != DBNull.Value ? Convert.ToDecimal(src["precio"]) : (decimal?)null)
           .Member(dest => dest.TipoIGV, src => src["tipoIGV"] != DBNull.Value ? Convert.ToInt32(src["tipoIGV"]) : (int?)null)
           .Member(dest => dest.PIGV, src => src["pIGV"] != DBNull.Value ? Convert.ToDecimal(src["pIGV"]) : (decimal?)null)
           .Member(dest => dest.FechaVencProducto, src => src["fechaVencProducto"] != DBNull.Value ? Convert.ToDateTime(src["fechaVencProducto"]) : (DateTime?)null)
           .Member(dest => dest.Api, src => src["api"] != DBNull.Value ? Convert.ToDecimal(src["api"]) : (decimal?)null)
           .Member(dest => dest.Temperatura, src => src["temperatura"] != DBNull.Value ? Convert.ToDecimal(src["temperatura"]) : (decimal?)null)
           .Member(dest => dest.IgvCosto, src => src["igvCosto"] != DBNull.Value ? Convert.ToDecimal(src["igvCosto"]) : (decimal?)null)
           .Member(dest => dest.SerieProducto, src => src["serieProducto"] != DBNull.Value ? src["serieProducto"].ToString() : null)
           .Member(dest => dest.Estado, src => src["estado"] != DBNull.Value ? Convert.ToInt32(src["estado"]) : (int?)null)
           .Member(dest => dest.NomProductoSrc, src => src["nomProductoSrc"] != DBNull.Value ? src["nomProductoSrc"].ToString() : null)
           .Member(dest => dest.IdRecepcionSrc, src => src["idRecepcionSrc"] != DBNull.Value ? src["idRecepcionSrc"].ToString() : null)
           .Member(dest => dest.IdPeriodo, src => src["idPeriodo"] != DBNull.Value ? Convert.ToInt32(src["idPeriodo"]) : (int?)null)
           .Member(dest => dest.FechaPeriodo, src => src["fechaPeriodo"] != DBNull.Value ? Convert.ToDateTime(src["fechaPeriodo"]) : (DateTime?)null)
           .Member(dest => dest.FechaImportacion, src => src["FechaImportacion"] != DBNull.Value ? Convert.ToDateTime(src["FechaImportacion"]) : (DateTime?)null)
           .Member(dest => dest.Actualizar, src => src["actualizar"] != DBNull.Value ? Convert.ToBoolean(src["actualizar"]) : (bool?)null)
           .Member(dest => dest.NCompraErp, src => src["nCompraErp"] != DBNull.Value ? src["nCompraErp"].ToString() : null)
           .Member(dest => dest.Scop, src => src["Scop"] != DBNull.Value ? src["Scop"].ToString() : null)
           ;

    }


}