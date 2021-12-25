using System.Collections.Generic;
using events;
using Lean.Touch;
using UnityEngine;

public class MenuSwipeProcessor : MonoBehaviour
{
    public Camera cam;

    public void HandleLeanEvent(List<LeanFinger> fingers, float delta)
    {
        Debug.Log($"float: {delta}, fingers: {fingers.Count}");
        var center = LeanGesture.GetLastScreenCenter(fingers);
        var pos = cam.ScreenToWorldPoint(new Vector3(center.x, center.y, 10));
        SwipeMenuEvents.Current.SwipeUp(pos);
    }
}