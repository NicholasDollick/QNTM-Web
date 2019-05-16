using System.Threading.Tasks;

namespace QNTM.API.Data
{
    public interface IValuesClient
    {
        Task Add(string value);

        Task Delete(string value);
    }
}