using System;
using UnityEngine;

namespace slicing.models
{
    [Serializable]
    public class SliceItem
    {
        public Vector3 initialLocalPosition;
        public Vector3 initialScale;
        public Quaternion initialRotation;
    }
}