using HarmonyLib;
using SapioxClient.API;
using System;

namespace SapioxClient.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.RpcRoundStarted))]
    internal static class RoundStart
    {
        [HarmonyPrefix]
        private static void OnRoundStart()
        {
            try
            {
                Handlers.Round.OnRoundStart();
            }
            catch (Exception e)
            {
                SapioxManager.log.LogError($"{typeof(RoundStart).FullName}.{nameof(OnRoundStart)}:\n{e}");
            }
        }
    }
}
