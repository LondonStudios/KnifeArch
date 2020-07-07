using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Server : BaseScript
    {
        public Server()
        {

            // If "remove" = true, remove it from the dictionary
            EventHandlers["Server:SyncArch"] += new Action<Vector3, int, bool>((location, id, remove) =>
            {
                TriggerClientEvent("Client:SyncArch", location, id, remove);
            });

            EventHandlers["Server:KnifeFound"] += new Action<Vector3>((location) =>
            {
                TriggerClientEvent("Client:KnifeFound", location);
            });
        }


    }
}
