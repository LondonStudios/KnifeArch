using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnifeArch
{
    public class Main : BaseScript
    {
        public int knifeArch = 0;
        public int knifeArchNetId = 0;
        public bool knifeResult = false;

        public float soundVolume = 0.6f;
        public float soundRadius = 30.0f;
        public List<uint> detectedWeapons = new List<uint>();

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
                    if (detectedWeapons.Count > 0)
                    {
                        Tick += DetectionThread;
                    }
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

            EventHandlers["Client:ArchCommand"] += new Action<string>((action) =>
            {
                if (action == "setup")
                {
                    SetupArch();
                }
                else
                {
                    RemoveArch();
                }
            });

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
                            Tick += DetectionThread;
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

        private async Task DetectionThread()
        {
            if (detectedWeapons.Count == 0 && !knifeResult)
            {
                Tick -= DetectionThread;
                return;
            }

            if (archDatabase.Count == 0)
            {
                if (knifeResult)
                {
                    knifeResult = false;
                    ShowNotification("No ~b~walk-through ~w~metal detectors setup, use ~b~/archdetect true ~w~again when ready.");
                }

                Tick -= DetectionThread;
                return;
            }

            int ped = PlayerPedId();
            Vector3 pos = GetEntityCoords(ped, true);

            foreach (KeyValuePair<Vector3, int> arch in archDatabase)
            {
                if (Vector3.DistanceSquared(pos, arch.Key) < 1.6f)
                {
                    if (knifeResult || HasPedGotDetectedWeapon(ped))
                    {
                        TriggerServerEvent("Server:KnifeFound", GetEntityCoords(PlayerPedId(), true));
                        ShowNotification("You have ~g~activated ~w~a ~b~walk-through ~w~metal detector.");
                        await Delay(1000);
                    }
                }
            }
            await Delay(250);
        }

        private bool HasPedGotDetectedWeapon(int ped)
        {
            foreach (uint weapon in detectedWeapons)
            {
                if (HasPedGotWeapon(ped, weapon, false))
                {
                    return true;
                }
            }
            return false;
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
                await RequestProp("ch_prop_ch_metal_detector_01a", true);
                knifeArch = CreateObject(GetHashKey("ch_prop_ch_metal_detector_01a"), coords.X, coords.Y, groundZ, true, true, true);
                await RequestProp("ch_prop_ch_metal_detector_01a", false);

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

        private async Task RequestProp(string model, bool load)
        {
            uint hash = (uint)GetHashKey(model);
            if (load)
            {
                while (!HasModelLoaded(hash))
                {
                    RequestModel(hash);
                    await Delay(0);
                }
            }
            else
            {
                SetModelAsNoLongerNeeded(hash);
            }
        }

        private void ConfigReader()
        {
            var data = LoadResourceFile(GetCurrentResourceName(), "config.ini");
            Configuration loaded = Configuration.LoadFromString(data);
            soundVolume = loaded["KnifeArch"]["SoundVolume"].FloatValue;
            soundRadius = loaded["KnifeArch"]["SoundRadius"].FloatValue;

            string[] weapons = loaded["KnifeArch"]["DetectedWeapons"].StringValueArray;
            foreach (string weapon in weapons)
            {
                detectedWeapons.Add((uint)GetHashKey(weapon));
            }

            if (soundVolume > 1.0f)
            {
                soundVolume = 1.0f;
            }
        }
    }
}