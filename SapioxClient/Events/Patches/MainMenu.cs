using HarmonyLib;
using SapioxClient.API;
using SapioxClient.Components;
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
    public class MainMenu
    {
        [HarmonyPatch(typeof(NewMainMenu), nameof(NewMainMenu.Start))]
        [HarmonyPrefix]
        public static bool OnMainMenuStart()
        {
            try
            {
                SapioxManager.log.LogInfo("Main Menu hooked!");
                var obj = new GameObject();
                obj.AddComponent<SapioxMenuWorker>();
                //var texture = new Texture2D(256, 256);
                //texture.LoadImage(File.ReadAllBytes(Path.Combine(SapioxManager.SapioxDirectory, "logo.png")));
                //GameObject.Find("Canvas/Logo").GetComponent<RawImage>().texture = texture;

                GameObject.Find("Canvas/Version").GetComponent<Text>().text = "11.1.2 (modded)";
                GameObject.Find("Canvas/PrivateBeta").GetComponent<TMPro.TMP_Text>().text = $"PRIVATE BETA - Sapiox Client (Version {SapioxManager.SapioxVersion})";

                if (Client._redirectCallback != null)
                {
                    Client._redirectCallback.Invoke();
                    Client._redirectCallback = null;
                }

                Handlers.Client.OnMainMenuStart();

                return true;
            }
            catch (Exception e)
            {
                SapioxManager.log.LogError($"{typeof(MainMenu).FullName}.{nameof(OnMainMenuStart)}:\n{e}");
                return true;
            }
        }
    }
}
