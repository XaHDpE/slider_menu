using Lean.Common;
using UnityEngine;


public class RotationBlocker : MonoBehaviour
{
private Vector3 oldEulerAngles;
private bool rotationEnabled;
private LeanManualRotate lmr;

private void Start(){
     oldEulerAngles = transform.rotation.eulerAngles;
     if (TryGetComponent(out LeanManualRotate lmrInt))
     {
          lmr = lmrInt;
     }
}

public void Rotate(Vector2 dir)
{
     if (!rotationEnabled) return;
     lmr.RotateAB(dir);
}


private void Update(){
     if (oldEulerAngles == transform.rotation.eulerAngles)
     {
          if (rotationEnabled) return;
          rotationEnabled = true;
     } else {
          oldEulerAngles = transform.rotation.eulerAngles;
          if (!rotationEnabled) return;
          rotationEnabled = false;
     }
}
}
