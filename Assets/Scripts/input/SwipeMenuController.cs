using System.Collections.Generic;
using DG.Tweening;
using input.model;
using Lean.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace input
{
    public class SwipeMenuController : MonoBehaviour
    {
        [SerializeField] private Transform elementsTop;
        private List<SliderItem> _elements;
        private List<Vector3> _placeholders;
        
        private Bounds _bounds;
        private LeanThresholdDelta _ltd;
        private float _sizeY;

        public int _head, _maxElementsCount;

        public Vector3 _spawnP1;

        // Start is called before the first frame update

        private void OnEnable()
        {
            _ltd = GetComponent<LeanThresholdDelta>();
        }

        private void Start()
        {
            _bounds = GetComponent<MeshRenderer>().bounds;
            _sizeY = _bounds.size.y;
            var startPoint = new Vector3(_bounds.min.x + _sizeY / 2, _bounds.center.y, _bounds.center.z);
            _maxElementsCount = (int) Mathf.Abs(_bounds.size.x / _sizeY);
            
            Debug.Log($"maxCount: {_maxElementsCount}");
            FillArray(30, _maxElementsCount);

            PlaceElements(_sizeY, startPoint, _elements, _maxElementsCount);

            _spawnP1 = startPoint;
            _ltd.Threshold = _sizeY;

            _head = 0;

        }


        private Dictionary<int, PrimitiveType> _listOfPrimitives = new Dictionary<int, PrimitiveType>()
        {
            {0, PrimitiveType.Cube}
            /*{1, PrimitiveType.Capsule},
            {2, PrimitiveType.Sphere}*/
        };

        public void ProcessDelta(float delta)
        {
            Debug.Log($"direction: {delta}");

            if (delta > 0)
            {
                // Shift(true);
            }
        }

        private void DistributeDelta()
        {
            
        }
        
        private void FillArray(int count, int maxCount)
        {
            _elements = new List<SliderItem>();

            for (var i = 0; i < count; i++)
            {
                var go = GameObject.CreatePrimitive(_listOfPrimitives[Random.Range(0, _listOfPrimitives.Count)]);
                go.name = $"element_{i}";
                go.transform.SetParent(elementsTop);
                go.transform.localPosition = Vector3.zero;
                go.AddComponent<SliderItem>();
                _elements.Add(go.GetComponent<SliderItem>());
            }

            var elementsTotal = _elements.Count;
            
            for (var i = 0; i < elementsTotal; i++)
            {
                // items[i].next = items[i == (items.Length - 1) ? 0 : i + 1];
                // items[i].previous = items[i == 0 ? items.Length - 1 : i - 1];

                var nextIdx = 0;
                if ((i - maxCount) < 0)
                    nextIdx = _elements.Count - Mathf.Abs(i - maxCount);
                else
                    nextIdx = i - maxCount;

                _elements[i].next = _elements[nextIdx];
                
                Debug.Log($"element: {_elements[i]}, nextIdx: {nextIdx}, maxIndex: {_elements.Count}");
                
                // items[i].next = items[(i - maxCount)%items.Length];
                // Debug.Log($"element: {items[i]}, prev: {items[i].previous}, next: {items[i].next}");
            }
            
        }

        private void PlaceElements(float size, Vector3 startPoint, List<SliderItem> all, int maxElementsCount)
        {
            for (var i = 0; i < maxElementsCount; i++)
            {
                var curIndex = i;
                var curObj = all[curIndex];
                var curTrn = curObj.transform;
                var curScale = curTrn.localScale;
                curTrn.SetParent(null);
                curTrn.position = new Vector3(startPoint.x + size * i, startPoint.y, startPoint.z);
                curTrn.localScale = new Vector3( curScale.x / size, curScale.y / size, curScale.z / size );
                curTrn.SetParent(transform);
                // curObj.EnableParentListeners();
            }
        }

        private void Instant(SliderItem element)
        {
            var trn = element.transform;
            trn.gameObject.SetActive(true);
            var curScale = trn.localScale;
            trn.localScale = new Vector3(curScale.x / _sizeY, curScale.y / _sizeY, curScale.z / _sizeY );
            trn.SetParent(transform);
            // trn.position = _spawnP1;
            // element.EnableParentListeners();
        }

        private void Hide(SliderItem element)
        {
            Debug.Log($"{element} is hidden");
            element.gameObject.SetActive(false);
            element.transform.SetParent(elementsTop);
            // element.DisableParentListeners();
        }
        
    }
    
}
