using PknoPlusCS.Configuration.Enum;
using PknoPlusCS.Global.ApiClient;
using PknoPlusCS.Global.DtoGlobales;
using PknoPlusCS.Modules.ValidationSunat.Application.Port;
using PknoPlusCS.Modules.ValidationSunat.Domain.Dto;
using PknoPlusCS.Modules.ValidationSunat.Domain.Repository;
using PknoPlusCS.Modules.ValidationSunat.Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace PknoPlusCS.Modules.ValidationSunat.Application.Adapter
{
    internal class ValidationSunatAdapter : IValidationSunatInputPort
    {

        private readonly IRepositoryValidationSunat _repo;
        private readonly IApiClient _apiClient;

        public ValidationSunatAdapter()
        {
            _repo = new ValidationSunatRepository();
            _apiClient = new ApiClient();
        }

        public async Task<ValidacionResultadoDto> ValidarSunatAsync(List<ListCpesDto> listData, string receptorRuc)
        {
            var resultado = new ValidacionResultadoDto();

            if (listData == null || listData.Count == 0)
            {
                resultado.Mensaje = "No hay datos para validar.";
                return resultado;
            }

            var comprobantesAValidar = listData
                .Where(x => x.EstadoSunat == "SIN VALIDAR")
                .ToList();

            if (comprobantesAValidar.Count == 0)
            {
                resultado.Mensaje = "No hay comprobantes pendientes de validar.";
                return resultado;
            }

            resultado.TotalProcesados = comprobantesAValidar.Count;

            foreach (var comprobante in comprobantesAValidar)
            {
                try
                {
                    var request = new ValidarComprobanteRequestDto
                    {
                        ReceptorRUC = receptorRuc,
                        EmisorRUC = comprobante.Ruc,
                        TipoDocumento = EstadoSunatHelper.ObtenerCodigoTipoDocumento(comprobante.TipoComprobante),
                        NumeroSerie = $"{comprobante.Serie}-{comprobante.NCorrelativo}",
                        FechaEmision = comprobante.FechaEmision.ToString("yyyy-MM-ddTHH:mm:ss"),
                        ImporteTotal = comprobante.ImporteSoles > 0
                            ? comprobante.ImporteSoles
                            : comprobante.ImporteDolares
                    };

                    var response = await _apiClient.ValidarComprobanteSunatAsync(request);

                    if (!response.TieneError && response.Resultado != null)
                    {
                        string nuevoEstado = EstadoSunatHelper.ObtenerEstadoParaBD(response.Resultado.EstadoCp);

                        int filasAfectadas = _repo.ActualizarEstadoSunatCPE(comprobante.NumDocPlus, nuevoEstado);

                        if (filasAfectadas > 0)
                        {
                            // Actualizar en memoria
                            comprobante.EstadoSunat = nuevoEstado;
                            comprobante.CantidadValidaciones++;
                            resultado.Exitosos++;
                        }
                        else
                        {
                            resultado.Errores++;
                        }
                    }
                    else
                    {
                        resultado.Errores++;
                    }

                    // Pequeña pausa para no saturar el API
                    await Task.Delay(100);
                }
                catch (Exception)
                {
                    resultado.Errores++;
                }
            }

            resultado.Mensaje = $"Validación completada. Exitosos: {resultado.Exitosos}, Errores: {resultado.Errores}";
            return resultado;
        }

        public string ExportarExcel(List<ListCpesDto> listData, string receptorRuc, int mes, int anio)
        {
            if (listData == null || listData.Count == 0)
                return null;

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Validación SUNAT");

                    var colorVerde = XLColor.FromHtml("#006400");
                    var colorVerdeClaro = XLColor.FromHtml("#90EE90");
                    var colorAmarillo = XLColor.FromHtml("#FFFF00");

                    int filaActual = 1;

                    worksheet.Cell(filaActual, 1).Value = "VALIDACIÓN DE CPEs DE COMPRAS - SUNAT";
                    worksheet.Range(filaActual, 1, filaActual, 11).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetBold(true)
                        .Font.SetFontSize(14)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    filaActual++;

                    worksheet.Cell(filaActual, 1).Value = "PECANO";
                    worksheet.Range(filaActual, 1, filaActual, 11).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetBold(true)
                        .Font.SetFontSize(12)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    filaActual++;

                    worksheet.Cell(filaActual, 1).Value = $"RUC Receptor: {receptorRuc}";
                    worksheet.Range(filaActual, 1, filaActual, 11).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetFontSize(10)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    filaActual++;

                    string nombreMes = ObtenerNombreMes(mes);
                    worksheet.Cell(filaActual, 1).Value = $"Período: {nombreMes} {anio}";
                    worksheet.Range(filaActual, 1, filaActual, 11).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetFontSize(10)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    filaActual++;

                    worksheet.Cell(filaActual, 1).Value = $"Fecha de Generación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                    worksheet.Range(filaActual, 1, filaActual, 11).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetFontSize(9)
                        .Font.SetItalic(true)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    filaActual += 2;


                    var encabezados = new string[]
                    {
                        "Nro.",
                        "Tipo Comprobante",
                        "N° Comprobante",
                        "Estado Compro.",
                        "RUC Emisor",
                        "Razón Social",
                        "Fecha Emisión",
                        "Moneda",
                        "Importe Soles (S/)",
                        "Importe Dólares ($)",
                        "Estado SUNAT"
                    };

                    for (int i = 0; i < encabezados.Length; i++)
                    {
                        var cell = worksheet.Cell(filaActual, i + 1);
                        cell.Value = encabezados[i];
                        cell.Style
                            .Font.SetBold(true)
                            .Font.SetFontColor(XLColor.White)
                            .Fill.SetBackgroundColor(colorVerde)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }

                    worksheet.Row(filaActual).Height = 25;
                    int filaEncabezados = filaActual;
                    filaActual++;


                    int filaInicioDatos = filaActual;
                    int numero = 1;

                    foreach (var item in listData)
                    {
                        worksheet.Cell(filaActual, 1).Value = numero;
                        worksheet.Cell(filaActual, 2).Value = item.TipoComprobante;
                        worksheet.Cell(filaActual, 3).Value = item.NroComprobante;
                        worksheet.Cell(filaActual, 4).Value = item.EstadoCompro;
                        worksheet.Cell(filaActual, 5).Value = item.Ruc;
                        worksheet.Cell(filaActual, 6).Value = item.RazonSocial;
                        worksheet.Cell(filaActual, 7).Value = item.FechaEmision.ToString("dd/MM/yyyy");
                        worksheet.Cell(filaActual, 8).Value = item.Moneda;
                        worksheet.Cell(filaActual, 9).Value = item.ImporteSoles;
                        worksheet.Cell(filaActual, 10).Value = item.ImporteDolares;
                        worksheet.Cell(filaActual, 11).Value = item.EstadoSunat;

                        worksheet.Cell(filaActual, 9).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Cell(filaActual, 10).Style.NumberFormat.Format = "#,##0.00";

                        worksheet.Cell(filaActual, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(filaActual, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(filaActual, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(filaActual, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        worksheet.Cell(filaActual, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        worksheet.Cell(filaActual, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        var cellEstado = worksheet.Cell(filaActual, 11);
                        switch (item.EstadoSunat?.ToUpper())
                        {
                            case "ACEPTADO":
                                cellEstado.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#90EE90"));
                                break;
                            case "NO EXISTE":
                            case "ANULADO":
                                cellEstado.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FF6B6B")); 
                                break;
                            case "SIN VALIDAR":
                                cellEstado.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#D3D3D3")); 
                                break;
                            default:
                                cellEstado.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFD700")); 
                                break;
                        }
                        cellEstado.Style.Font.SetBold(true);

                        for (int col = 1; col <= 11; col++)
                        {
                            worksheet.Cell(filaActual, col).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        }

                        numero++;
                        filaActual++;
                    }

                    int filaFinDatos = filaActual - 1;


                    worksheet.Cell(filaActual, 1).Value = "TOTALES";
                    worksheet.Range(filaActual, 1, filaActual, 8).Merge();
                    worksheet.Cell(filaActual, 1).Style
                        .Font.SetBold(true)
                        .Fill.SetBackgroundColor(colorAmarillo)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(filaActual, 9).FormulaA1 = $"SUM(I{filaInicioDatos}:I{filaFinDatos})";
                    worksheet.Cell(filaActual, 9).Style
                        .Font.SetBold(true)
                        .Fill.SetBackgroundColor(colorAmarillo)
                        .NumberFormat.SetFormat("#,##0.00")
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(filaActual, 10).FormulaA1 = $"SUM(J{filaInicioDatos}:J{filaFinDatos})";
                    worksheet.Cell(filaActual, 10).Style
                        .Font.SetBold(true)
                        .Fill.SetBackgroundColor(colorAmarillo)
                        .NumberFormat.SetFormat("#,##0.00")
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(filaActual, 11).Style.Fill.SetBackgroundColor(colorAmarillo);

                    for (int col = 1; col <= 11; col++)
                    {
                        worksheet.Cell(filaActual, col).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }

                    filaActual += 2;


                    worksheet.Cell(filaActual, 1).Value = "RESUMEN DE ESTADOS";
                    worksheet.Cell(filaActual, 1).Style.Font.SetBold(true);
                    filaActual++;

                    var resumenEstados = listData
                        .GroupBy(x => x.EstadoSunat ?? "SIN ESTADO")
                        .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                        .OrderBy(x => x.Estado)
                        .ToList();

                    foreach (var estado in resumenEstados)
                    {
                        worksheet.Cell(filaActual, 1).Value = estado.Estado;
                        worksheet.Cell(filaActual, 2).Value = estado.Cantidad;

                        switch (estado.Estado?.ToUpper())
                        {
                            case "ACEPTADO":
                                worksheet.Cell(filaActual, 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#90EE90"));
                                break;
                            case "NO EXISTE":
                            case "ANULADO":
                                worksheet.Cell(filaActual, 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FF6B6B"));
                                break;
                            case "SIN VALIDAR":
                                worksheet.Cell(filaActual, 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#D3D3D3"));
                                break;
                        }

                        filaActual++;
                    }

                    worksheet.Cell(filaActual, 1).Value = "Total Registros:";
                    worksheet.Cell(filaActual, 1).Style.Font.SetBold(true);
                    worksheet.Cell(filaActual, 2).Value = listData.Count;
                    worksheet.Cell(filaActual, 2).Style.Font.SetBold(true);


                    worksheet.Column(1).Width = 8;   
                    worksheet.Column(2).Width = 20;   
                    worksheet.Column(3).Width = 18;   
                    worksheet.Column(4).Width = 14;   
                    worksheet.Column(5).Width = 14;   
                    worksheet.Column(6).Width = 35;   
                    worksheet.Column(7).Width = 14;   
                    worksheet.Column(8).Width = 10;  
                    worksheet.Column(9).Width = 18;  
                    worksheet.Column(10).Width = 18;  
                    worksheet.Column(11).Width = 15;



                    string carpeta = @"C:\TXT Pecano\VALIDEZ CPE";
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    string nombreArchivo = $"ValidacionSunat_{nombreMes}_{anio}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    workbook.SaveAs(rutaCompleta);

                    return rutaCompleta;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al exportar a Excel: {ex.Message}", ex);
            }
        }

        private string ObtenerNombreMes(int mes)
        {
            string[] meses = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                               "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            return mes >= 1 && mes <= 12 ? meses[mes] : mes.ToString();
        }
    }
}
