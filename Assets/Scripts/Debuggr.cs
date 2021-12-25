using UnityEngine;

public static class Debuggr
{
    public static void DrawPlane(Vector3 position, Vector3 normal, Color planeColor) {
 
        Vector3 v3;
        float dur = 100;
        //var planeColor = Color.cyan;
        // var normColor = Color.blue;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
 
        Debug.DrawLine(corner0, corner2, planeColor, dur);
        Debug.DrawLine(corner1, corner3, planeColor, dur);
        Debug.DrawLine(corner0, corner1, planeColor, dur);
        Debug.DrawLine(corner1, corner2, planeColor, dur);
        Debug.DrawLine(corner2, corner3, planeColor, dur);
        Debug.DrawLine(corner3, corner0, planeColor, dur);
        Debug.DrawRay(position, normal, planeColor, dur);
    }
}
