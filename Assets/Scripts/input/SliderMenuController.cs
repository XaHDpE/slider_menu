using System.Collections.Generic;
using DG.Tweening;
using events;
using helpers;
using input.model;
using Lean.Common;
using Lean.Touch;
using settings;
using UnityEngine;

namespace input
{
    public class SliderMenuController : MonoBehaviour
    {
        [SerializeField] private Transform counterpart;

        // private LeanThresholdDelta _ltd;
        public float remainingDelta, step;
        public SliderItem headSi;
        public LinkedList<SliderItem> itemsReduced;
        private Vector3 _eastSp, _westSp, slideDirection;
        private bool initialized;
    
        private void Start()
        {
            CustomGameEvents.Current.OnObjectSliced += ObjectSlicedHandler;
            // CustomGameEvents.Current.OnSliderItemIsOut += RecalculateArray;
            // _ltd = GetComponent<LeanThresholdDelta>();
            // AlignRotation();
            // CalculateDimensions();
        }

        private void OnDestroy()
        {
            CustomGameEvents.Current.OnObjectSliced -= ObjectSlicedHandler;
        }


        
        
        public void RegisterDelta(Vector2 delta)
        {
            remainingDelta = delta.x;
            // headSi.Move(remainingDelta);
            DistributeDelta(remainingDelta);
        }

        private void DistributeDelta(float deltaX)
        {
            foreach (var t in itemsReduced)
            {
                t.Move(deltaX);
            }
        }


        private static Vector3 GetSpawnPoint(bool isWest, Vector3 initPoint, float offset)
        {
            return initPoint +
                      Vector3.right * (isWest ? offset : -offset) / 2 +
                      Vector3.up * offset / 2 +
                      Vector3.back * offset / 2;
        }

        private void ObjectSlicedHandler(Transform tr)
        {
            var cam = GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>();
            var numOfCubes = SettingsReader.Sms.numberOfCubes;
            var dist = Vector3.Distance(cam.transform.position, counterpart.position) / 2;
            var frCorners = ScreenHelper.FrustumCorners(cam, dist);
            _westSp = GetSpawnPoint(true, frCorners[0], 0);
            _eastSp = GetSpawnPoint(false, frCorners[3], 0);
            var stepSize = Vector3.Distance(frCorners[0], frCorners[3]) /  numOfCubes;
            step = stepSize;
            slideDirection = (_eastSp - _westSp).normalized;
            var items = FillArray(tr);
            
            itemsReduced = PlaceItems(CalculatePlaces(_westSp, _eastSp, numOfCubes), items, stepSize);
            
            Debug.Log($"itemsReduced: {itemsReduced.Count}");
            
            UpdateSequence(items, numOfCubes);

            initialized = true;

            // DrawSlider(transform.InverseTransformPoint(startPoint), stepSize, itemsReduced);
            // UpdateLeanParameters(stepSize);
            // AlignPositionRotation();
        }
        
        /*private void AlignPositionRotation()
        {
            var lpy = GetComponentInParent<LeanPitchYaw>();
            transform.localEulerAngles = new Vector3(0, lpy.Yaw, 0);
        }*/
        
        private List<SliderItem> FillArray(Transform itemsTop)
        {
            var cubes = itemsTop.GetComponentsInChildren<CubeController>();
            var items = new List<SliderItem>();
            var allowedDistance = Vector3.Distance(_eastSp, _westSp) / 2;
            var screenCenter = _westSp + (_eastSp - _westSp).normalized * allowedDistance;
        
            for (var index = 0; index < cubes.Length; index++)
            {
                var cube = cubes[index];
                var si = cube.gameObject.AddComponent<SliderItem>();
                si.allowedDistance = allowedDistance;
                si.screenCenter = screenCenter;
                si.gameObject.SetActive(false);
                si.transform.SetParent(transform);
                si.name += $"_{index}";
                si.GetComponent<LeanManualTranslate>().DirectionA = slideDirection;
                CopyHelper.MoveHierarchyToLayer(si.transform, SettingsReader.Sms.menuLayer);
                items.Add(si);
            }
            return items;
        }

        private void UpdateSequence(List<SliderItem> items, int maxVisible)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var nextToSpawnIdx = (items.Count - 1 + i) % items.Count;
                var prevToDropIdx = (items.Count + maxVisible + i) % items.Count;
                items[i].next = items[nextToSpawnIdx];
                items[i].previous = items[prevToDropIdx];
                // items[i].idx = i;
                // Debug.Log($"i: {items[i]}, items.nextToSpawn: {items[i].nextToSpawnIdx}, items.prevToDrop: {items[i].prevToDropIdx}");
            }
        }

        private List<Vector3> CalculatePlaces(Vector3 west, Vector3 east, int num)
        {

            var distance = Vector3.Distance(west, east);
            var itStep = distance / (num - 1);
            var res = new List<Vector3>(num);
            var dir = (east - west).normalized;
            for (var i = 0; i < num; i++)
            {
                res.Add(west + dir * itStep * i);
            }

            return res;
        }

        private void RecalculateArray(SliderItem outSi)
        {
            
            itemsReduced.Remove(outSi);
        }

        private SliderItem SpawnElement(SliderItem si, Vector3 pos, float size)
        {
            ScaleItem(si.transform, size, 0.8f);
            si.transform.position = pos;
            si.gameObject.SetActive(true);
            return si.GetComponent<SliderItem>();
        }

        private void DropElement(SliderItem si)
        {
            si.gameObject.SetActive(false);
        }
        
        private LinkedList<SliderItem> PlaceItems(IReadOnlyList<Vector3> places, IReadOnlyList<SliderItem> allItems, float elementSize)
        {
            var res = new LinkedList<SliderItem>();
            for (var i = 0; i < places.Count; i++)
            {
                var cEl = SpawnElement(allItems[i], places[i] + Vector3.up * elementSize / 2, elementSize);
                res.AddLast(cEl);
            }
            headSi = res.First.Value;
            return res;
        }

        // ONLY CUBES
        private void ScaleItem(Transform si, float finalScale, float multiplier)
        {
            var itemScale = si.localScale;
            var nSize = new Vector3(
                finalScale / itemScale.x * multiplier,
                finalScale / itemScale.y * multiplier, 
                finalScale / itemScale.z * multiplier) / 2;
            si.transform.localScale = nSize;
        }

        /*private void UpdateLeanParameters(float stepX)
        {
            _ltd.Threshold = stepX;
        }*/
        
        private void Update()
        {
            if (!initialized) return;

            // MoveAll();
            
            if (!(Vector3.Distance(headSi.transform.position, _westSp) >= step)) return;
            // var nl = SpawnElement(headSi.next, _westSp + Vector3.up * step / 2, step);
            headSi = SpawnElement(headSi.next, _westSp + Vector3.up * step / 2, step);
            DropElement(itemsReduced.Last.Value);
            // itemsReduced.AddLast(nl);
            itemsReduced.AddFirst(headSi);
            itemsReduced.RemoveLast();
        }

        private void MoveAll()
        {
            var currItem = itemsReduced.First.Next;
            var i = 1;
            while (currItem != null)
            {
                currItem.Value.transform.position = itemsReduced.First.Value.transform.position + slideDirection * (i * step);
                currItem = currItem.Next;
                i++;
            }
        }
    }
}
