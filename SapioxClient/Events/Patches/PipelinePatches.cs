using HarmonyLib;
using SapioxClient.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;

namespace SapioxClient.Events.Patches
{
    [HarmonyPatch(typeof(GameConsoleTransmission), nameof(GameConsoleTransmission.Start))]
    internal static class PipelinePatches
    {
        public static GameConsoleTransmission Transmission = null;

        [HarmonyPrefix]
        public static bool OnStart(GameConsoleTransmission __instance)
        {
            Transmission = __instance;
            return true;
        }


        [HarmonyPatch(typeof(GameConsoleTransmission), nameof(GameConsoleTransmission.TargetPrintOnConsole))]
        [HarmonyPrefix]
        public static bool OnConsolePrint(Il2CppStructArray<byte> data, bool encrypted)
        {
            if (data == null) return false;
            if (encrypted || !DataUtils.IsData(data)) return true;
            ClientPipeline.Receive(DataUtils.Unpack(data));
            return false;
        }
    }
}
