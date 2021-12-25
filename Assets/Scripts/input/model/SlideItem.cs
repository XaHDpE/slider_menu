using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace input.model
{
    [RequireComponent(typeof(LeanManualTranslate))]
    [RequireComponent(typeof(LeanManualSwipe))]
    public class SlideItem : MonoBehaviour
    {
        private LeanManualTranslate _lmt;
        private LeanManualSwipe _lms;
        private LeanFingerDownCanvas _fingerCanvas;

        //TODO Remove this bond
        public void Init()
        {
            _lmt = GetComponent<LeanManualTranslate>();
            _lms = GetComponent<LeanManualSwipe>();            
        }

        public void EnableForeignListener()
        {
            var canvas = GetComponentInParent<SliderTop2>().touchCanvas;
            _fingerCanvas = canvas.GetComponent<LeanFingerDownCanvas>();
            _fingerCanvas.OnFinger.AddListener(Hop);
            // _lms.onDelta.AddListener(_lmt.TranslateAB);
            Debug.Log("listeners enabled");
        }

        private void OnDestroy()
        {
            _lms.onDelta.RemoveAllListeners();
            // fingerCanvas.OnFinger.RemoveAllListeners();
        }

        private void Hop(LeanFinger leanFinger)
        {
            _lms.AddFinger(leanFinger);
        }
        
    }
}