using HarmonyLib;
using RemoteAdmin;
using SapioxClient.API;
using SapioxClient.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.Events.Patches
{
    public class CommandLine
    {
        [HarmonyPatch(typeof(GameCore.Console), nameof(GameCore.Console.Awake))]
        [HarmonyPrefix]
        public static bool OnStart()
        {
            Log.Info("Starting GameConsole");
            return true;
        }

        [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.CmdSendEncryptedQuery))]
        [HarmonyPrefix]
        public static bool OnCmdQuery()
        {
            Log.Info("Sending Encrypted Query");
            return true;
        }

        [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.EcdsaSign))]
        [HarmonyPrefix]
        public static bool OnSign()
        {
            Log.Info("ECDSA Sign");
            return true;
        }

        [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.CmdSendEncryptedQuery))]
        [HarmonyPrefix]
        public static bool OnCmdQueryCall()
        {
            Log.Info("Encrypted Query Call");
            return true;
        }
    }
}
