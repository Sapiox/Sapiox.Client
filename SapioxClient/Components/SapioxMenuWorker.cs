using MelonLoader;
using MelonLoader.Support;
using SapioxClient.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.Components
{

    [RegisterTypeInIl2Cpp]
    public class SapioxMenuWorker : MonoBehaviour
    {
        public SapioxMenuWorker(IntPtr intPtr) : base(intPtr) { }

        //ReferenceHub.LocalHub.nicknameSync.UpdateNickname("Helight");
        public void Update()
        {
            try
            {
                Client.DoQueueTick();
                Coroutines.Process();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void FixedUpdate()
        {
            try
            {
                Coroutines.ProcessWaitForFixedUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void LateUpdate()
        {
            try
            {
                Coroutines.ProcessWaitForEndOfFrame();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void OnEnable()
        {

        }
    }
}
