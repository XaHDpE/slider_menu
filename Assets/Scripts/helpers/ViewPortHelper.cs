using UnityEngine;

namespace helpers
{
    public class ViewPortHelper
    {
        private Camera cam;
        private float depth;
        
        public ViewPortHelper(Camera cam, float depth)
        {
            this.cam = cam;
            this.depth = depth;
            DebugViewPort();
        }

        public void DebugViewPort()
        {
            var color = Color.cyan;
            var dur = 100;
            Debug.DrawLine(GetVpLeftBottom(), GetVpLeftTop(), color, dur);
            Debug.DrawLine(GetVpLeftTop(), GetVpRightTop(), color, dur);
            Debug.DrawLine(GetVpRightTop(), GetVpRightBottom(), color, dur);
            Debug.DrawLine(GetVpRightBottom(), GetVpLeftBottom(), color, dur);
        }

        public Vector3 GetVpLeftBottom()
        {
            return cam.ViewportToWorldPoint(new Vector3(0, 0, depth));
        }

        public Vector3 GetVpLeftTop()
        {
            return cam.ViewportToWorldPoint(new Vector3(0, 1, depth));
        }
        
        public Vector3 GetVpRightBottom()
        {
            return cam.ViewportToWorldPoint(new Vector3(1, 0, depth));
        }
        
        public Vector3 GetVpRightTop()
        {
            return cam.ViewportToWorldPoint(new Vector3(1, 1, depth));
        }

        public Vector3[] FrustumCorners()
        {
            var intArr = new Vector3[4];
            cam.CalculateFrustumCorners(
                new Rect(0, 0, 1, 1),
                depth,
                Camera.MonoOrStereoscopicEye.Mono,
                intArr
            );

            var res = new Vector3[intArr.Length];
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = cam.transform.TransformPoint(intArr[i]);
            }

            return res;
        }
        
        
        
    }
}