using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class SliderMenuSettings : ScriptableObject
    {
        [SerializeField] public int menuLayer = 6;
        [SerializeField] public int numberOfCubes = 10;
        public Vector3 westSpawnPoint = new Vector3(.1f, .1f, 10);
        public Vector3 eastSpawnPoint = new Vector3(.9f, .1f, 10);
        
    }
}
