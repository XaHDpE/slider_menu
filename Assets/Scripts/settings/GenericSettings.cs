using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class GenericSettings : ScriptableObject
    {
        [SerializeField] public GameObject shellPrefab;
        [SerializeField] public GameObject mapPointPrefab;
    }
}