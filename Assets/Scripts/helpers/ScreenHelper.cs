using UnityEngine;

namespace helpers
{
    public static class ScreenHelper
    {
        public static Vector2 CalculateFrustum(float fow, float distance)
        {
            var frustumHeight =  2.0f * distance * Mathf.Tan(fow * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * Screen.width / Screen.height;
            return new Vector2(frustumWidth, frustumHeight);
        }

        public static Vector3[] FrustumCorners(Camera cam, float distance)
        {
            var intArr = new Vector3[4];
            cam.CalculateFrustumCorners(
                new Rect(0, 0, 1, 1),
                distance,
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
        

        public static Vector3 GetVpRightBottom(Camera cam, float distance)
        {
            return cam.ViewportToWorldPoint(new Vector3(1, 0, distance));
        }
        
        public static Vector3 GetVpLeftBottom(Camera cam, float distance)
        {
            return cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
        }
        
        public static void ScaleRectangle(RectTransform myRect, float horizontalSize, float verticalSize)
        {
            //If you want the middle of the rect be somewhere else then the middle of the screen change it here (0 ... 1, 0 ... 1)
            var rectMiddle = new Vector2(0.5f, 0.5f);
 
            myRect.sizeDelta = Vector2.zero; //Dont want any delta sizes, because that would defeat the point of anchors
            myRect.anchoredPosition = Vector2.zero; //And the position is set by the anchors aswell so we set the offset to 0
 
            myRect.anchorMin = new Vector2(rectMiddle.x - horizontalSize / 2,
                rectMiddle.y - verticalSize / 2);
            myRect.anchorMax = new Vector2(rectMiddle.x + horizontalSize / 2,
                rectMiddle.y + verticalSize / 2);
        }
        
        public static bool CheckVisible(Camera cam, Vector3 pos)
        {
            return Screen.safeArea.Contains(cam.WorldToScreenPoint(pos));
        }
        
        
    }
}