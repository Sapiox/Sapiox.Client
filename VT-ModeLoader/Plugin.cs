using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using CommandSystem.Commands.Shared;
using HarmonyLib;

namespace VT_ModeLoader
{
    [BepInPlugin("VT", "VT-Loader", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public const int ClientMajor = 1;
        public const int ClientMinor = 0;
        public const int ClientPatch = 0;
        public const string ClientVersion = "1.0.0-Beta";
        public const string ClientDescription = "The client is a modificated version of the SCP:SL client for support mods";


        private static ManualLogSource log;
        public override void Load()
        {
            CustomNetworkManager.Modded = true;
            BuildInfoCommand.ModDescription = $"\n=====ClientMod=====\nClient: VT-ModeLoader\nClient Version: {ClientVersion}\nDescription: {ClientDescription}";

            log = Log;
            Log.LogInfo("Loaded Start test!");
            Harmony.CreateAndPatchAll(typeof(Plugin));
            Log.LogInfo("Applied hooks!");
        }

        [HarmonyPatch(typeof(CentralAuthManager), nameof(CentralAuthManager.Sign))]
        [HarmonyPrefix]
        static bool OnLaunchCommand(ref string __result, string ticket)
        {
            log.LogInfo($"Trying to sign: {ticket}");
            __result = "";
            return false;
        }
    }
}

