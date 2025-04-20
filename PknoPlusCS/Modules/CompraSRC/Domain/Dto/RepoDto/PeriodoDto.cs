using System;

namespace PknoPlusCS.Modules.CompraSRC.Domain.Dto.RepoDto
{
    public class PeriodoDto
    {
        public int IdPeriodo { get; set; }
        public string NomPeriodo { get; set; }
        public int IdTipoPeriodo { get; set; }
        public DateTime FechaI {  get; set; }
        public DateTime FechaFin { get; set; }
        public bool Cerrado { get; set; }
    }
}
