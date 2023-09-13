using System;
using cubes;
using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class SliderMenuSettings : ScriptableObject
    {
        [SerializeField] public int menuLayer = 6;
        [SerializeField] public int numberOfCubes = 10;
        public float relativeVpItemSize = 0.1f;
        public Vector3 westSpawnPoint = new Vector3(0f, .1f, 10);
        public Vector3 eastSpawnPoint = new Vector3(1f, .1f, 10);
        public float swipeSpeed = 0.005f;
        public float relativeItemInterval = 0.3f;
        public Type itemType = typeof(CubeController);
    }
}
