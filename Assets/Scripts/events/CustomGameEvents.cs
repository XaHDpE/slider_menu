using System;
using input;
using input.model;
using Lean.Touch;
using scroll.scroll.swipe;
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

        public event Action<RollItem> OnSetCurrentlyRunning;

        public void SetCurrentlyRunning(RollItem ri)
        {
            OnSetCurrentlyRunning?.Invoke(ri);
        }

    }
}
