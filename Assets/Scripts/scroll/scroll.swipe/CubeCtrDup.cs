using System;
using Lean.Common;
using Lean.Touch;
using scroll.scroll.swipe;
using UnityEngine;

[RequireComponent(typeof(LeanMultiUpdate))]
[RequireComponent(typeof(LeanManualTranslate))]
public class CubeCtrDup : MonoBehaviour
{
    private LeanMultiUpdate lmu;
    private LeanManualTranslate ldm;
    public CubeCtr prev;
    public CubeCtr next;
    public float offset;
    public bool head;
    public Camera cam;
    
    void Start()
    {
        lmu = GetComponent<LeanMultiUpdate>();
        ldm = GetComponent<LeanManualTranslate>();
    }

    private float CheckPrevPosition()
    {
        // minus - prev is on the left of given
        var res = cam.WorldToScreenPoint(transform.localPosition) - cam.WorldToScreenPoint(prev.transform.localPosition);
        return res.x;
    }

    private void OnDestroy()
    {
        lmu.OnDelta.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        if (head) return;
        transform.localPosition = prev.transform.localPosition - Vector3.right * offset;
    }
}
