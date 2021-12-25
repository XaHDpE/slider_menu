using events;
using Lean.Common;
using UnityEngine;

namespace input
{
    [RequireComponent(typeof(LeanManualTranslate))]
    public class RollItem : MonoBehaviour
    {
        private LeanManualTranslate _ltr;
        public int nextToSpawnIdx;
        public int idx;
        public int prevToDropIdx;
        public bool head;
        public float multiplier;

        private void Start()
        {
            _ltr = GetComponent<LeanManualTranslate>();
            GetComponent<Renderer>().sharedMaterial.color = Color.blue;
        }
        
        private void OnDestroy()
        {
            DisableMovementListener();
        }

        private void EnableMovementListener()
        {
            _ltr = GetComponent<LeanManualTranslate>();
            transform.GetComponentInParent<LeanThresholdDelta>().OnDeltaX.AddListener(OnDelta);
        }

        private void DisableMovementListener()
        {
            transform.GetComponentInParent<LeanThresholdDelta>().OnDeltaX.RemoveListener(OnDelta);
            var ltd = gameObject.GetComponentInParent(typeof(LeanThresholdDelta)) as LeanThresholdDelta;
            if (ltd != null) 
                transform.GetComponentInParent<LeanThresholdDelta>().OnDeltaX.RemoveListener(OnDelta);
        }

        public void CallMeHead()
        {
            Debug.Log($"i'm called head {transform}");
            EnableMovementListener();
            GetComponent<Renderer>().sharedMaterial.color = Color.red;
        }
        
        public void DontCallMeHead()
        {
            DisableMovementListener();
            GetComponent<Renderer>().sharedMaterial.color = Color.blue;
            Debug.Log($"no longer a head: {transform}");
        }


        public void Move(float delta)
        {
            _ltr.TranslateA(delta);
        }
        
        private void OnDelta(float delta)
        {
            CustomGameEvents.Current.SetCurrentlyRunning(this);
            _ltr.TranslateA(delta);
        }
    }
}