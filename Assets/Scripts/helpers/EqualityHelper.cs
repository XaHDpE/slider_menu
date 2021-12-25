using UnityEngine;

namespace helpers
{
    public static class EqualityHelper
    {
        public static bool Approximately(this Quaternion a, Quaternion b, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(a, b)) < acceptableRange;
        }
    }
}