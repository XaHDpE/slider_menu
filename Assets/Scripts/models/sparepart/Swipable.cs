using Lean.Touch;
using UnityEngine;

namespace models.sparepart
{
    public class Swipable : MonoBehaviour
    {
        
        // events and delegates
        public delegate void SwipeEventDelegate(Vector2 delta);
        public static event SwipeEventDelegate SwipeUp;
        public static event SwipeEventDelegate SwipeDown;
        public static event SwipeEventDelegate SwipeLeft;
        public static event SwipeEventDelegate SwipeRight;
        
        private LeanFingerSwipe _lfsUp;
        private LeanFingerSwipe _lfsLeft;
        private LeanFingerSwipe _lfsRight;
        private LeanFingerSwipe _lfsDown;

        private void Awake()
        {
            _lfsUp = gameObject.AddComponent<LeanFingerSwipe>();
            _lfsLeft = gameObject.AddComponent<LeanFingerSwipe>();
            _lfsDown = gameObject.AddComponent<LeanFingerSwipe>();
            _lfsRight = gameObject.AddComponent<LeanFingerSwipe>();

            ConfigureLeanScripts();

        }

        private void OnDestroy()
        {
            Destroy(_lfsDown);
            Destroy(_lfsLeft);
            Destroy(_lfsRight);
            Destroy(_lfsUp);
        }

        private void OnEnable()
        {
            _lfsUp.OnDelta.AddListener(InvokeSwipeUp);
            _lfsDown.OnDelta.AddListener(InvokeSwipeDown);
            _lfsLeft.OnDelta.AddListener(InvokeSwipeLeft);
            _lfsRight.OnDelta.AddListener(InvokeSwipeRight);
        }

        private void OnDisable()
        {
            _lfsUp.OnDelta.RemoveListener(InvokeSwipeUp);
            _lfsDown.OnDelta.RemoveListener(InvokeSwipeDown);
            _lfsLeft.OnDelta.RemoveListener(InvokeSwipeLeft);
            _lfsRight.OnDelta.RemoveListener(InvokeSwipeRight);
        }

        public void RemoveListeners()
        {
            OnDisable();
        }

        private void ConfigureLeanScripts()
        {
            // Configure Up
            _lfsUp.RequiredAngle = 0;
            _lfsUp.RequiredArc = 60;
            
            // Configure Down
            _lfsDown.RequiredAngle = 180;
            _lfsDown.RequiredArc = 60;
            
            // Configure Left
            _lfsLeft.RequiredAngle = 270;
            _lfsLeft.RequiredArc = 60;
            
            // Configure Right
            _lfsRight.RequiredAngle = 90;
            _lfsRight.RequiredArc = 60;
        }
        
        // invokers methods
        private static void InvokeSwipeUp(Vector2 delta)
        {
            SwipeUp?.Invoke(delta);
        }

        private static void InvokeSwipeRight(Vector2 delta)
        {
            SwipeRight?.Invoke(delta);
        }
        
        private static void InvokeSwipeDown(Vector2 delta)
        {
            SwipeDown?.Invoke(delta);
        }

        private static void InvokeSwipeLeft(Vector2 delta)
        {
            SwipeLeft?.Invoke(delta);
        }
        
    }
}