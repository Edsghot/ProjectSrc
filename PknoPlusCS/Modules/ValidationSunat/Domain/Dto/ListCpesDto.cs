using System;

namespace PknoPlusCS.Modules.ValidationSunat.Domain.Dto
{
    public class ListCpesDto
    {
        public int Id { get; set; }
        public string NumDocPlus { get; set; }
        public string TipoComprobante { get; set; }
        public string Serie { get; set; }
        public string NCorrelativo { get; set; }
        public string NroComprobante { get; set; }
        public string EstadoCompro { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Moneda { get; set; }
        public decimal ImporteSoles { get; set; }
        public decimal ImporteDolares { get; set; }
        public int CantidadValidaciones { get; set; }
        public string EstadoSunat { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}