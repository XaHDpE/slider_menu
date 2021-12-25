using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class SliderMenuSettings : ScriptableObject
    {
        [SerializeField] public int menuLayer = 6;
        [SerializeField] public int numberOfCubes = 10;
    }
}