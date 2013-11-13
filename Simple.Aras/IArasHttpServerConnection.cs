using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simple.Aras
{
    public interface IArasHttpServerConnection
    {
        Task<XElement> ApplyAmlAsync(XElement item, string action = "ApplyAML");
    }
}