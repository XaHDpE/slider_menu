using cubes;
using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class GenericSettings : ScriptableObject
    {
        public GameObject shellPrefab;
        public GameObject mapPointPrefab;
        public CubeController[] cubes;
        public CubeController[] cubesFiltered;
    }
}