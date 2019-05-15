using Microsoft.AspNetCore.SignalR.Core;
using System.Threading.Tasks;
using QNTM.API.Data;

namespace QNTM.API.Hubs
{
    public class ValuesHub : Hub<IValuesClient>
    {
        public async Task Add(string value) => await Clients.All.PostValue(value);

        public async Task Delete(string value) => await Clients.All.DeleteValue(value);
    }
}