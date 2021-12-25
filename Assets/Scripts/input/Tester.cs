using System;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private LeanManualTranslate lmt;
    private LeanMultiUpdate lmu;
    public Vector3 startPosition;
    private Transform _target, _etalon;

    private float _distanceFromStart, _size;
    private Transform head;
    
    void Start()
    {
        lmt = GetComponent<LeanManualTranslate>();
        lmu = GetComponent<LeanMultiUpdate>();
        var transform1 = transform; 
        startPosition = transform1.position;
        _target = transform1;
        _etalon = transform1;
        _size = transform1.GetComponent<MeshRenderer>().bounds.size.x * 1.2f;
    }

    [SerializeField] public float currentDelta, currentDistance;

    public float rd;
    public bool issueEvent, instantiated;

    public void RegisterDelta(Vector2 delta)
    {
        foreach (var trn in GetComponentsInChildren<Transform>())
        {
            if (trn.Equals(transform)) continue;
            trn.GetComponent<LeanManualTranslate>().TranslateA(delta.x);
        }
    }

    private void PropagateDelta(float delta)
    {
        
    }

    private void Update()
    {
        // remove and add participants 
        // call to PropagateDelta
    }
}
