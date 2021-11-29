using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SapioxClient.Events.Patches
{
    [HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.Start))]
    internal static class MainMenuStart
    {
        [HarmonyPrefix]
        public static bool OnMainMenuStart()
        {
            try
            {
                SapioxManager.log.LogInfo("Main Menu hooked!");

                /*var texture = new Texture2D(256, 256);
                ImageConversion.LoadImage(texture, File.ReadAllBytes(Path.Combine(SapioxManager.SapioxDirectory, "logo.png")), false);
                GameObject.Find("Canvas/Logo").GetComponent<RawImage>().texture = texture;*/
                Handlers.Client.OnMainMenuStart();

                return true;
            }
            catch (Exception e)
            {
                SapioxManager.log.LogError($"{typeof(MainMenuStart).FullName}.{nameof(OnMainMenuStart)}:\n{e}");
                return true;
            }
        }
    }
}
