using System.Threading.Tasks;

namespace app_matter_data_src_erp.Modules.CompraSRC.Application.Port
{
    public interface ICompraSrcInputPort
    {
        Task ObtenerDataSrc();
    }
}