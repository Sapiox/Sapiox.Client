using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.Components
{

    [RegisterTypeInIl2Cpp]
    public class SapioxSpawned : MonoBehaviour
    {
        public SapioxSpawned(IntPtr intPtr) : base(intPtr) { }
        public Il2CppSystem.String Blueprint { get; internal set; }

        private Transform _transform;

        public void Awake()
        {
            _transform = transform;
        }

        public void Update()
        {

        }

        //Reserved
        public void TweenTo(Vector3 vector3, Quaternion quaternion)
        {
            _transform.position = vector3;
            _transform.rotation = quaternion;
        }

        public static SapioxSpawned ForObject(GameObject gameObject)
        {
            var ss = gameObject.GetComponent<SapioxSpawned>();
            if (ss == null)
            {
                throw new Exception("GameObject is not spawned via Sapiox");
            }

            return ss;
        }
    }
}
