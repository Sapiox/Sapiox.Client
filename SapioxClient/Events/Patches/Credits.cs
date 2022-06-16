using HarmonyLib;
using SapioxClient.API;
using SapioxClient.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.Events.Patches
{
    public class Credits
    {
        [HarmonyPatch(typeof(NewCredits), nameof(NewCredits.OnEnable))]
        [HarmonyPrefix]
        public static bool InjectCreditsHook()
        {
            GameObject creditsHookObject = new GameObject
            {
                name = "Credits Hook"
            };
            //creditsHookObject.AddComponent<CreditsHook>();

            return true;
        }
    }
}
