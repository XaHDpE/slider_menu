using System;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace cubes
{
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanManualRotate))]
    [RequireComponent(typeof(LeanThresholdDelta))]
    public class Rotatable : MonoBehaviour
    {
        private LeanMultiUpdate lmu;
        private LeanManualRotate lmr;
        private LeanThresholdDelta ltd;

        private void Start()
        {
            if (TryGetComponent(out LeanMultiUpdate lmuInt))
            {
                lmu = lmuInt;
            }

            if (TryGetComponent(out LeanManualRotate lmrInt))
            {
                lmr = lmrInt;
            }

            if (TryGetComponent(out LeanThresholdDelta ltdInt))
            {
                ltd = ltdInt;
            }
        }
    }
}