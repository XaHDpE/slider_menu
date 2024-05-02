using cubes;
using UnityEngine;
using UnityEngine.UI;

namespace settings
{
    [CreateAssetMenu]
    public class GenericSettings : ScriptableObject
    {
        public GameObject shellPrefab;
        public GameObject mapShellPrefab;
        public GameObject mapPointPrefab;
        public GameObject navSlider;
        public CubeController[] cubes;
        public CubeController[] cubesFiltered;
    }
}