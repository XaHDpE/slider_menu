using System;
using Lean.Touch;
using scroll.scroll.swipe;
using UnityEngine;

namespace events
{
    public class SwipeMenuEvents : MonoBehaviour
    {
        
        public static SwipeMenuEvents Current;

        private void Awake()
        {
            Current = this;
            Debug.Log("CustomGameEvents initialized");
        }
        
        public event Action<CubeCtr> OnHeadChanged;
        
        public void HeadChanged(CubeCtr cc)
        {
            OnHeadChanged?.Invoke(cc);
        }
        
        public event Action<LeanFinger> OnFingerDown;
        
        public void FingerDownMenu(LeanFinger lf)
        {
            OnFingerDown?.Invoke(lf);
        }

        public event Action<Vector2> OnHorizontalDrag;

        public void HorizontalDrag(Vector2 delta)
        {
            OnHorizontalDrag?.Invoke(delta);
        }

        public event Action<Vector3> OnSwipeUp;

        public void SwipeUp(Vector3 pos)
        {
            OnSwipeUp?.Invoke(pos);
        }

    }
}