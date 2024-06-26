using System;
using UnityEngine;

namespace events
{
    public class CustomGameEvents : MonoBehaviour
    {
        public static CustomGameEvents Current;

        private void Awake()
        {
            Current = this;
            Debug.Log("CustomGameEvents initialized");
        }

        public event Action<Transform, Vector3Int> OnObjectSliced;
        public event Action<Transform> OnMapCreated;

        public void MapCreated(Transform top)
        {
            OnMapCreated?.Invoke(top);
        }
        
        public void ObjectSliced(Transform top, Vector3Int slicesCount)
        {
            OnObjectSliced?.Invoke(top, slicesCount);
        }

        public event Action<Transform> OnShuffleDone;
        
        public void ShuffleDone(Transform top)
        {
            OnShuffleDone?.Invoke(top);
        }

        public event Action<Transform> OnCarouselTopSet;

        public void CarouselTopSet(Transform carouselTop)
        {
            OnCarouselTopSet?.Invoke(carouselTop);
        }

        public event Action<Transform> OnMainCameraSet;

        public void MainCameraSet(Transform mainCam)
        {
            OnMainCameraSet?.Invoke(mainCam);
        }
        
    }
}
