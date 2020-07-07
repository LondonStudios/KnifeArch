using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using SharpConfig;
using static CitizenFX.Core.Native.API;

namespace KnifeArch
{
    public class Main : BaseScript
    {
        public int knifeArch = 0;
        public int knifeArchNetId = 0;
        public bool knifeResult = false;

        public bool permissionsEnabled = false;
        public float soundVolume = 0.6f;
        public float soundRadius = 30.0f;

        public int blip;

        public Dictionary<Vector3, int> archDatabase = new Dictionary<Vector3, int> { };

        public Main()
        {
            ConfigReader();

            EventHandlers["Client:SyncArch"] += new Action<Vector3, int, bool>((location, id, remove) =>
            {
                if (!remove)
                {
                    archDatabase.Add(location, id);
                }
                else
                {
                    if (archDatabase.ContainsKey(location))
                    {
                        archDatabase.Remove(location);
                    }
                }
            });

            EventHandlers["Client:KnifeFound"] += new Action<Vector3>((targetCoords) =>
            {
                Vector3 playerCoords = GetEntityCoords(PlayerPedId(), true);
                var distance = Vdist(playerCoords.X, playerCoords.Y, playerCoords.Z, targetCoords.X, targetCoords.Y, targetCoords.Z);
                float distanceVolumeMultiplier = (soundVolume / soundRadius);
                float distanceVolume = soundVolume - (distance * distanceVolumeMultiplier);
                if (distance <= soundRadius)
                {
                    SendNuiMessage(string.Format("{{\"submissionType\":\"knifeArch\", \"submissionVolume\":{0}, \"submissionFile\":\"{1}\"}}", (object)distanceVolume, (object)"knifearch.mp3"));
                    ShowNotification("~r~CAUTION: ~w~Walk-Through Metal Detector ~r~Activated~w~.");
                }
            });

            RequestProp("ch_prop_ch_metal_detector_01a");

            TriggerEvent("chat:addSuggestion", "/arch", "Setup or remove a knife arch", new[]
            {
                new { name="Action", help="setup/remove" },
            });

            TriggerEvent("chat:addSuggestion", "/archdetect", "Toggle knife arch detection, set result", new[]
            {
                new { name="Detect", help="true/false" },
            });

            RegisterCommand("archdetect", new Action<int, List<object>, string>((source, args, raw) =>
            {
                try
                {
                    var arg = Convert.ToString(args[0]);
                    if (arg.ToLower() == "true")
                    {
                        if (archDatabase.Keys.Count < 1)
                        {
                            ShowNotification("Please try again when a knife arch has been setup.");
                        }
                        else
                        {
                            knifeResult = true;
                            KnifeDetection();
                            ShowNotification("You will now activate a ~b~walk-through ~w~metal detector.");
                        } 
                    }
                    else if (arg.ToLower() == "false")
                    {
                        knifeResult = false;

                        ShowNotification("You will not activate a ~b~walk-through ~w~metal detector.");
                    }
                }
                catch
                {
                    ProcessError("Usage /archdetect [true/false].");
                }
            }), false);
        }

        private async void KnifeDetection()
        {
            while (knifeResult)
            {
                if (archDatabase.Keys.Count < 1)
                {
                    await Delay(20000);
                    if (archDatabase.Keys.Count < 1)
                    {
                        knifeResult = false;
                        ShowNotification("No ~b~walk-through ~w~metal detectors setup, use ~b~/archdetect true ~w~again when ready.");
                    }
                }
                var coords = GetEntityCoords(PlayerPedId(), true);
                foreach (KeyValuePair<Vector3, int> kvp in archDatabase)
                {
                    if (coords.DistanceToSquared(kvp.Key) < 1.6f)
                    {
                        TriggerServerEvent("Server:KnifeFound", GetEntityCoords(PlayerPedId(), true));
                        ShowNotification("You have ~g~activated ~w~a ~b~walk-through ~w~metal detector.");
                        await Delay(1000);
                        break;
                    }
                }
                await Delay(350);
            }
        }

        private void RemoveArch()
        {
            if (!(knifeArch == 0))
            {
                var coords = GetEntityCoords(knifeArch, true);
                var netid = knifeArchNetId;
                DeleteEntity(ref knifeArch);
                knifeArch = 0;
                knifeArchNetId = 0;
                TriggerServerEvent("Server:SyncArch", coords, netid, true);
                ShowNotification("~b~Walk-through ~w~metal detector removed.");
            }
            else
            {
                ProcessError("No knife arch found to remove");
            }
        }

        private void SetupArch()
        {
            if (!IsEligible())
            {
                SpawnArch();
                ShowNotification("~b~Walk-through ~w~metal detector setup.");
            }
            else
            {
                ProcessError("You are unable to setup a knife arch.");
            }
        }

        private async void SpawnArch()
        {
            float groundZ = 0.0f;
            var coords = GetEntityCoords(PlayerPedId(), true);
            var ground = GetGroundZFor_3dCoord(coords.X, coords.Y, coords.Z, ref groundZ, false);
            if (groundZ == 0)
            {
                ProcessError("You are unable to setup a knife arch here.");
            }
            else
            {
                knifeArch = CreateObject(GetHashKey("ch_prop_ch_metal_detector_01a"), coords.X, coords.Y, groundZ, true, true, true);
                var networkId = ObjToNet(knifeArch);
                SetNetworkIdExistsOnAllMachines(networkId, true);
                SetNetworkIdCanMigrate(networkId, false);
                NetworkSetNetworkIdDynamic(networkId, true);
                knifeArchNetId = networkId;
                SetEntityHeading(knifeArch, GetEntityHeading(PlayerPedId()));
                TriggerServerEvent("Server:SyncArch", GetEntityCoords(knifeArch, true), knifeArchNetId, false);

                blip = AddBlipForEntity(knifeArch);
                SetBlipAsFriendly(blip, true);
                SetBlipColour(blip, 38);
                SetBlipDisplay(blip, 2);
                AddTextEntry("knifearch", "Knife Arch");
                BeginTextCommandSetBlipName("knifearch");
                EndTextCommandSetBlipName(blip);
            }

            await Delay(100);
        }

        private void ProcessError(string message = "You are unable to setup a knife arch here.")
        {
            PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                args = new[] { "[KnifeArch]", $"{message}" }
            });
        }

        private void ShowNotification(string text)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(text);
            EndTextCommandThefeedPostTicker(false, false);
        }

        private bool IsEligible()
        {
            if (IsPedInAnyVehicle(PlayerPedId(), true) || knifeArch == 0)
            {
                return false;
            }
            return true;
        }

        private async void RequestProp(string model)
        {
            RequestModel((uint)GetHashKey(model));
            while (!HasModelLoaded((uint)GetHashKey(model)))
            {
                await Delay(100);
            }
        }

        private void ConfigReader()
        {
            var data = LoadResourceFile(GetCurrentResourceName(), "config.ini");
            if (Configuration.LoadFromString(data).Contains("KnifeArch", "PermissionsEnabled") == true)
            {
                Configuration loaded = Configuration.LoadFromString(data);
                permissionsEnabled = loaded["KnifeArch"]["PermissionsEnabled"].BoolValue;
                soundVolume = loaded["KnifeArch"]["SoundVolume"].FloatValue;
                soundRadius = loaded["KnifeArch"]["SoundRadius"].FloatValue;

                if (soundVolume > 1.0f)
                {
                    soundVolume = 1.0f;
                }
                CreateCommand(permissionsEnabled);
            }
            else
            {
                CreateCommand(false);
            }
        }

        private void CreateCommand(bool permissions)
        {
            RegisterCommand("arch", new Action<int, List<object>, string>((source, args, raw) =>
            {
                try
                {
                    var arg = Convert.ToString(args[0]);

                    if (arg.ToLower() == "setup")
                    { 
                        SetupArch();
                    }
                    else if (arg.ToLower() == "remove")
                    {
                        RemoveArch();
                    }
                }
                catch
                {
                    ProcessError("Usage /arch [setup/remove].");
                }
            }), permissions);
        }
    }
}
