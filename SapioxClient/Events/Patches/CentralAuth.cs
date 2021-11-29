using GameCore;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Events.Patches
{
    [HarmonyPatch(typeof(CentralAuthManager), nameof(CentralAuthManager.Sign))]
    internal static class Sing
    {
        [HarmonyPrefix]
        public static bool OnSing(ref string __result, string ticket)
        {
            try
            {
                SapioxManager.log.LogInfo($"Trying to sign: {ticket}");
                __result = "";
                return false;
            }
            catch (Exception e)
            {
                SapioxManager.log.LogError($"{typeof(CentralAuth).FullName}.{nameof(OnSing)}:\n{e}");
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(CentralAuthManager), nameof(CentralAuthManager.Authentication))]
    internal static class Auth
    {
        [HarmonyPrefix]
        public static bool OnAuth()
        {
            SapioxManager.log.LogInfo("Faking Central Server Data");
            CentralAuthManager.Abort();
            CentralAuthManager.InitAuth();
            CentralAuthManager.ApiToken = "";
            CentralAuthManager.Authenticated = true;
            CentralAuthManager.AuthStatusType = AuthStatusType.Success;
            CentralAuthManager.Nonce = "Sapiox";

            Handlers.Client.OnCentralAuth();

            return true;
        }
    }
}
