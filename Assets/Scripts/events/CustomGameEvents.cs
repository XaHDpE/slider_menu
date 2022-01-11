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

        public event Action<Transform> OnObjectSliced;

        public void ObjectSliced(Transform top)
        {
            OnObjectSliced?.Invoke(top);
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
