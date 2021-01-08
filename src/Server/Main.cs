using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using SharpConfig;
using System;
using System.Collections.Generic;

namespace KnifeArch
{
    public class Main : BaseScript
    {
        public Main()
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

            string data = LoadResourceFile(GetCurrentResourceName(), "config.ini");
            Configuration config = Configuration.LoadFromString(data);
            bool usePermissions = config["KnifeArch"]["PermissionsEnabled"].BoolValue;

            RegisterCommand("arch", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count > 0)
                {
                    string action = Convert.ToString(args[0]).ToLower();
                    if (action == "setup" || action == "remove")
                    {
                        Player target = Players[source];
                        TriggerClientEvent(target, "Client:ArchCommand", action);
                    }
                }
            }), usePermissions);
        }
    }
}