using System;
using UnityEngine;

namespace helpers
{
    public static class MathExtended
    {
        public static float GetPolygonSideSizeByCircle(float radius, int numOfSides)
        {
            return 2 * radius * Mathf.Sin(Mathf.PI / numOfSides);
        }

        public static float GetPolygonSideNumByCircle(float radius, float polySideSize)
        {
            return 180 / (Mathf.Rad2Deg * Mathf.Asin(polySideSize));
        }

        public static float GetCubeDiagonal(float side)
        {
            return side * Mathf.Sqrt(3);
        }
        
        public static float ClosestDivisible(float n, float m)
        {
            var q = n / m;
            var n1 = m * q;
            var n2 = (n * m) > 0 ? (m * (q + 1)) : (m * (q - 1));
            return Math.Abs(n - n1) < Math.Abs(n - n2) ? n1 : n2;
        }
        
    }
}