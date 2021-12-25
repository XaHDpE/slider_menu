using System.Collections.Generic;
using events;
using input;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

public class SliderTop : MonoBehaviour
{
    public int currentlyRunningIdx, prevHeadIdx, maxVisible;
    public Vector3 _startPos;
    public float _itemSize;
    public List<RollItem> _items;
    public Material headMaterial;
    public Material def;
    
    private void Start()
    {

        CustomGameEvents.Current.OnSetCurrentlyRunning += RegisterCurrentlyRunning;
        
        var bounds = transform.GetComponent<MeshRenderer>().bounds;
        maxVisible = 5;
        _itemSize = bounds.size.y;

        GetComponent<LeanThresholdDelta>().Threshold = _itemSize;

        _items = FillArray(30, maxVisible);
        _startPos = CalculateStartPoint(bounds);
        PlaceElements(_itemSize, _startPos, _items, maxVisible);
    }


    
    public Vector3 prevPos;
    public float remainingDelta, remainingDistance, currentDistance;
    
    public void RegisterDelta(float delta)
    {
        remainingDelta = delta;
    } 
    
    void RegisterCurrentlyRunning(RollItem ri)
    {
        currentlyRunningIdx = ri.idx;
    }

    public float distancePassed;
    
    private void Update()
    {

        // move all objects in accordance with head
        for (var i = 1; i < maxVisible; i++)
        {
            var curIdx = (currentlyRunningIdx + i) % _items.Count;
            var curObj = _items[curIdx];
            curObj.transform.position = _items[currentlyRunningIdx].transform.position + Vector3.right * _itemSize * i;
        }
        
        // calculate remaining distance
        distancePassed = Vector3.Distance(_items[currentlyRunningIdx].transform.position, _startPos);

        if (Vector3.Distance(_items[currentlyRunningIdx].transform.position, _startPos) >= _itemSize) {
            var currRi = Spawn(_items[currentlyRunningIdx].nextToSpawnIdx);
            //currRi.Move(remainingDelta);
        }

        if (Vector3.Distance(transform.position, prevPos) < 0.0001)
        {
            // distancePassed = 0;
            // remainingDelta = 0;
        }
            

        prevPos = _items[currentlyRunningIdx].transform.position;
    }

    RollItem Spawn(int index)
    {
        _items[index].transform.position = _startPos;
        _items[index].gameObject.SetActive(true);
        return _items[index];
    }

    void Hide(int index)
    {
        _items[index].gameObject.SetActive(false);
    }
    
    private Vector3 CalculateStartPoint(Bounds b)
    {
        var sizeY = b.size.y;
        return new Vector3(b.min.x, b.center.y, b.center.z);
    }
    
    private List<RollItem> FillArray(int count, int maxVisible)
    {
        var items = new List<RollItem>();

        for (var i = 0; i < count; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = $"element_{i}";
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<RollItem>();
            go.SetActive(false);
            items.Add(go.GetComponent<RollItem>());
            
        }
        
        for (var i = 0; i < items.Count; i++)
        {
            var nextToSpawnIdx = (items.Count - 1 + i) % items.Count;
            var prevToDropIdx = (items.Count + maxVisible + i) % items.Count;
            items[i].nextToSpawnIdx = nextToSpawnIdx;
            items[i].prevToDropIdx = prevToDropIdx;
            items[i].idx = i;
            // Debug.Log($"i: {items[i]}, items.nextToSpawn: {items[i].nextToSpawnIdx}, items.prevToDrop: {items[i].prevToDropIdx}");
        }

        return items;
    }
    
    private void PlaceElements(float size, Vector3 startPoint, List<RollItem> all, int maxElementsCount)
    {
        for (var i = 0; i < maxElementsCount; i++)
        {
            var curIndex = i;
            var curObj = all[curIndex];
            var curTrn = curObj.transform;
            // var curScale = curTrn.localScale;

            curTrn.gameObject.SetActive(true);
            curTrn.SetParent(null);
            curTrn.position = new Vector3(startPoint.x + size * i, startPoint.y, startPoint.z);
            // curTrn.localScale = new Vector3( curScale.x / size, curScale.y / size, curScale.z / size );
            curTrn.SetParent(transform);
        }

        currentlyRunningIdx = 0;
        _items[currentlyRunningIdx].CallMeHead();
    }
    
    
}
