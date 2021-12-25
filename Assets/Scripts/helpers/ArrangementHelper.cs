using System;
using UnityEngine;

namespace helpers
{
    public static class ArrangementHelper
    {
        public static Tuple<Vector3, Quaternion> GetPositionInCircle(Vector3 parentPosition, int idx, int totalItems, float radius)
        {
            var angle = idx * Mathf.PI * 2 / totalItems;
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;
            var pos = parentPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle * Mathf.Rad2Deg;
            return new Tuple<Vector3, Quaternion>(pos, Quaternion.Euler(0, angleDegrees, 0));
        }
        
        public static void GetPositionInCircle(
            Vector3 parentPosition, int idx, int totalItems, float radius, Action<Vector3, Quaternion> callback
            )
        {
            var angle = idx * Mathf.PI * 2 / totalItems;
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;
            var pos = parentPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle * Mathf.Rad2Deg;
            callback(pos, Quaternion.Euler(0, angleDegrees, 0));
        }
    
        public static Tuple<Vector3, Quaternion> GetPositionInCircleWithSelected(
            Vector3 parentPosition, 
            int idx, 
            int totalItems,
            int sel2IdleRel,
            float radius)
        {

            Debug.Log($"work with index: {idx}, total count: {totalItems}, rel: {sel2IdleRel}");
        
            if (idx > 0 && idx <= Mathf.Ceil((sel2IdleRel-1)/2) || idx >= totalItems - Mathf.Ceil((sel2IdleRel-1)/2))
            {
                Debug.Log($"skipped index: {idx}, total count: {totalItems}, rel: {sel2IdleRel}");
                return new Tuple<Vector3, Quaternion>(Vector3.negativeInfinity, Quaternion.identity);
            };
        
            var angle = idx * Mathf.PI * 2 / totalItems;
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;
            var pos = parentPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle * Mathf.Rad2Deg;
            return new Tuple<Vector3, Quaternion>(pos, Quaternion.Euler(0, angleDegrees, 0));
        }
    
        public static void GetPositionInCircleWithSelected(
            Vector3 parentPosition, 
            int idx, 
            int totalItems,
            int sel2IdleRel,
            float radius,
            Action<Vector3, Quaternion> callBack
        )
        {

            Debug.Log($"work with index: {idx}, total count: {totalItems}, rel: {sel2IdleRel}");
        
            if (idx > 0 && idx <= Mathf.Ceil((sel2IdleRel-1)/2) || idx >= totalItems - Mathf.Ceil((sel2IdleRel-1)/2))
            {
                Debug.Log($"skipped index: {idx}, total count: {totalItems}, rel: {sel2IdleRel}");
                return;
            };
        
            var angle = idx * Mathf.PI * 2 / totalItems;
            var x = Mathf.Sin(angle) * radius;
            var z = Mathf.Cos(angle) * radius;
            var pos = parentPosition + new Vector3(x, 0, z);
            var angleDegrees = -angle * Mathf.Rad2Deg;
            callBack(pos, Quaternion.Euler(0, angleDegrees, 0));
        }
    
    
    }
}