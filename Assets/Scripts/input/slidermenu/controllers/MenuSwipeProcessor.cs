using System.Collections.Generic;
using events;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace input.slidermenu.controllers
{
    public class MenuSwipeProcessor : MonoBehaviour
    {
        public Camera cam;

        public void HandleLeanEvent(List<LeanFinger> fingers, float delta)
        {
            Debug.Log($"delta: {delta}");
            var center = LeanGesture.GetLastScreenCenter(fingers);
            var pos = cam.ScreenToWorldPoint(new Vector3(center.x, center.y, 10));
            SwipeMenuEvents.Current.SwipeUp(pos);
            
            var lms = transform.GetComponent<LeanManualSwipe>();
            
            lms.AddFinger(fingers[0]);
            SwipeMenuEvents.Current.SwipeUp(pos);

        }

        public void HandleWorldFrom(Vector3 delta)
        {
            SwipeMenuEvents.Current.SwipeUp(delta);
        }
        
        public void HandleDelta(Vector2 delta)
        {
            Debug.Log($"delta is : {delta.y}");
        }
        
    }
}