using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace input.model
{
    [RequireComponent(typeof(LeanManualTranslate))]
    [RequireComponent(typeof(LeanSelectableByFinger))] 
    public class SliderItem : MonoBehaviour
    {
        private LeanManualTranslate _ltr;
        public SliderItem previous;
        public SliderItem next;
        public Vector3 screenCenter;
        public float allowedDistance;

        private void Start()
        {
            _ltr = GetComponent<LeanManualTranslate>();
            _ltr.Damping = 10;
        }

        private void OnDestroy()
        {
            DisableParentListeners();
        }

        public void EnableParentListeners()
        {
            // transform.GetComponentInParent<LeanThresholdDelta>().OnDeltaX.AddListener(OnDelta);
        }

        public void DisableParentListeners()
        {
            /*var ltd = gameObject.GetComponentInParent(typeof(LeanThresholdDelta)) as LeanThresholdDelta;
            if (ltd != null) 
                transform.GetComponentInParent<LeanThresholdDelta>().OnDeltaX.RemoveListener(OnDelta);*/
        }

        public void Move(float delta)
        {
            _ltr = GetComponent<LeanManualTranslate>();
            _ltr.TranslateA(delta);
        }

        private void Update()
        {
            /*if (Vector3.Distance(screenCenter, transform.position) <= allowedDistance) return;
            CustomGameEvents.Current.SliderItemIsOut(this);
            gameObject.SetActive(false);*/
        }
    }
}