using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Events.Patches
{
    public class GlobalPermissions
    {
        private readonly static List<ServerRoles> requestBadge = new List<ServerRoles>();

        [HarmonyPatch(typeof(ServerRoles), nameof(ServerRoles.Update))]
        [HarmonyPrefix]
        public static bool OnUpdate(ServerRoles __instance)
        {
            if (__instance.CurrentColor == null) return false;

            if (!string.IsNullOrEmpty(__instance.FixedBadge) && __instance.MyText != __instance.FixedBadge)
            {
                __instance.SetText(__instance.FixedBadge);
                __instance.SetColor("silver");
                return false;
            }

            if (!string.IsNullOrEmpty(__instance.FixedBadge) && __instance.CurrentColor.Name != "silver")
            {
                __instance.SetColor("silver");
                return false;
            }

            if (__instance.GlobalBadge != __instance._prevBadge)
            {
                __instance._prevBadge = __instance.GlobalBadge;

                if (string.IsNullOrEmpty(__instance.GlobalBadge))
                {
                    __instance._bgc = null;
                    __instance._bgt = null;
                    __instance._authorizeBadge = false;
                    __instance._prevColor += ".";
                    __instance._prevText += ".";
                    return false;
                }

                requestBadge.Add(__instance);
                GlobalBadge(__instance);
            }

            if (__instance._prevColor == __instance.MyColor && __instance._prevText == __instance.MyText) return false;

            if (requestBadge.Contains(__instance))
            {
                return false;
            }

            if (__instance.CurrentColor.Restricted &&
                (__instance.MyText != __instance._bgt || __instance.MyColor != __instance._bgc))
            {
                GameCore.Console.AddLog(
                    $"TAG FAIL 1 - {__instance.MyText} - {__instance._bgt} /-/ {__instance.MyColor} - {__instance._bgc}",
                    UnityEngine.Color.gray, false);
                __instance._authorizeBadge = false;
                __instance.Network_myColor = "default";
                __instance.Network_myText = null;
                __instance._prevColor = "default";
                __instance._prevText = null;
                PlayerList.UpdatePlayerRole(__instance.gameObject);
                return false;
            }

            if (__instance.MyText != null && __instance.MyText != __instance._bgt && (__instance.MyText.Contains("[") ||
                __instance.MyText.Contains("]") || __instance.MyText.Contains("<") || __instance.MyText.Contains(">")))
            {
                GameCore.Console.AddLog(
                    $"TAG FAIL 2 - {__instance.MyText} - {__instance._bgt} /-/ {__instance.MyColor} - {__instance._bgc}",
                    UnityEngine.Color.gray, false);
                __instance._authorizeBadge = false;
                __instance.Network_myColor = "default";
                __instance.Network_myText = null;
                __instance._prevColor = "default";
                __instance._prevText = null;
                PlayerList.UpdatePlayerRole(__instance.gameObject);
                return false;
            }

            __instance._prevColor = __instance.MyColor;
            __instance._prevText = __instance.MyText;
            __instance._prevBadge = __instance.GlobalBadge;
            PlayerList.UpdatePlayerRole(__instance.gameObject);

            return false;
        }

        private static void GlobalBadge(ServerRoles ply)
        {
            var su = /*await SynapseCentral.Get.Resolve(ply._hub.characterClassManager.UserId)*/ "Naku@Sapiox";

            /*if (su.Groups == null || su.Groups.Count < 1)
            {
                ply._bgc = null;
                ply._bgt = null;
                ply._authorizeBadge = false;
                ply._prevColor += ".";
                ply._prevText += ".";
                requestBadge.Remove(ply);
                return;
            }

            var group = su.Groups[0];

            if (group.Color == "(none)" || group.Name == "(none)")
            {
                ply._bgc = null;
                ply._bgt = null;
                ply._authorizeBadge = false;
            }
            else
            {
                ply.Network_myText = group.Name;
                ply._bgt = group.Name;

                ply.Network_myColor = group.Color;
                ply._bgc = group.Color;

                ply._authorizeBadge = true;
            }*/

            ply.Network_myText = "Sapiox Developer";
            ply._bgt = "Sapiox Developer";

            ply.Network_myColor = "purple";
            ply._bgc = "purple";

            ply._authorizeBadge = true;

            requestBadge.Remove(ply);
        }
    }
}
