using System;
using System.Collections.Generic;
using converters;
using DG.Tweening;
using events;
using helpers;
using Lean.Common;
using states.controllers;
using UnityEngine;
using bh = helpers.BoundsHelper;
using th = helpers.TransformHelper;
using sr = settings.SettingsReader;

namespace input
{
    public class Carousel : MonoBehaviour
    {
        private Transform _carouselElementsParent;
        [SerializeField] private Transform topElement;
        

        private void Start()
        {
            _arrangementDone = false;
            CustomGameEvents.Current.OnObjectSliced += OnObjectSliced;
            // TopBarSparePartState.ItemIsBackToCarousel += AddItem;
        }

        private void OnDestroy()
        {
            CustomGameEvents.Current.OnObjectSliced -= OnObjectSliced;
            // TopBarSparePartState.ItemIsBackToCarousel -= AddItem;
        }
        
        // events
        public void OnDeltaX(float deltaX)
        {
            
        }

        private float _alpha;
        private int _currNumTmp;
        private bool _arrangementDone;

        [SerializeField] private float deltaAngle;
        [SerializeField] private int currNum, prevNum;
        [SerializeField] private SparePartStateManager[] carouselItems;
        
        
        public void RegisterDelta(float deltaX)
        {
            
            var curRotation = Mathf.Abs(transform.rotation.y);

            Debug.Log($"currRot: {curRotation}");
            
            if (Math.Abs(curRotation - 10f) < 0.001)
            {
                Debug.Log($"currRot: BINGO: {curRotation}");
                UpdateLeanThreshold(20f);
            } 
            
            /*Debug.Log($"deltaX: {deltaX}, closest divisible by {deltaAngle}: " +
                      $"{MathExtended.ClosestDivisible(deltaX, deltaAngle)}");
            Debug.DrawRay(_carouselElementsParent.position, _carouselElementsParent.forward * 20f, Color.red);*/
        }
        
        private void FixedUpdate()
        {
            if (!_arrangementDone) return;

            
            var newRot = transform.rotation;
            newRot.y = Camera.main.transform.parent.rotation.y;
            newRot.x = 0;
            
            transform.rotation = newRot;
            
            var offset = transform.eulerAngles.y - Camera.main.GetComponent<Transform>().eulerAngles.y;
            _alpha = (offset - 180 - deltaAngle * 1 / 2) % 360;
            
            // _alpha = (transform.eulerAngles.y + deltaAngle * 1 / 2) % 360;
            _currNumTmp = Mathf.RoundToInt((_alpha - _alpha % deltaAngle) / deltaAngle);
            currNum = (carouselItems.Length - _currNumTmp)%carouselItems.Length;
            
            Debug.Log($"alpha: {_alpha}, _currNumTmp: {_currNumTmp}, currNum {currNum}");
            
            if (currNum != prevNum)
            {
                if (prevNum > -1) carouselItems[prevNum].MoveToIdle();
                // RearrangementInProgress?.Invoke();
                carouselItems[currNum].MoveToSelectedInList();
            }
            else
            {
                /*if (!_eventEmitted)
                {
                    print("i should emit event");
                    // NewElementCameToCenter?.Invoke();
                    _eventEmitted = true;
                }#1#*/
            }

            prevNum = currNum;
        }
        
        private void OnObjectSliced(Transform parent)
        {
            _carouselElementsParent = parent;
            Setup();
        }
        
        private void Setup()
        {
            PlaceTop(topElement);
            SetForwardVectorInSettings();
            SetCarouselItems(_carouselElementsParent.GetComponentsInChildren<CubeController>());
            SetCarouselItemsFiltered(carouselItems);
            // SetCarouselItemsFilteredInSettings();
            MoveItemsUnderCarousel();
            SetCarouselRadius();
            SetItemIdleSize();
            SetItemSelectedSize();
            ArrangeInCircle();
            SetDeltaAngle(360f / carouselItems.Length);
            UpdateLeanThreshold(deltaAngle);
            _arrangementDone = true;
        }

        private void UpdateLeanThreshold(float da)
        {
            Debug.Log($"deltaAngle: {da}");
            GetComponent<LeanThresholdDelta>().Threshold = da;
        }
        
        private void MoveItemsUnderCarousel()
        {
            var i = 0;
            foreach (var ci in carouselItems)
            {
                ci.name = ci.name + "_"+ i++; 
                ci.transform.SetParent(transform);
            }
        }

        private void SetForwardVectorInSettings()
        {
            sr.Cs.topForwardVector = transform.forward;
        }
        
        private void SetDeltaAngle(float val)
        {
            deltaAngle = val;
        }

        private void SetCarouselItems(CubeController[] val)
        {
            var res = new List<SparePartStateManager>(Array.ConvertAll(
                val, 
                CubeToCarouselItemConverter.CubeToCarouselItem
            ));
            carouselItems = res.ToArray();
        }
        
        private void SetCarouselItemsFiltered(SparePartStateManager[] cis)
        {
            var res = new List<SparePartStateManager>();
            foreach (var cb in cis)
            {
                if (cb.GetComponent<CubeController>().IsEmpty())
                {
                    cb.GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    res.Add(cb);
                }
            }
            //carouselItemsFiltered = res.ToArray();
            carouselItems = res.ToArray();
        }
        private void PlaceTop(Transform top)
        {
            var currTransform = transform;
            currTransform.position = top.position;
            currTransform.rotation = top.rotation;
            // Debug.DrawRay(currTransform.position, currTransform.forward, Color.cyan, 100);
            CustomGameEvents.Current.CarouselTopSet(currTransform);
        }
        private void SetCarouselRadius()
        {
            sr.Cs.radius = bh.GetMaxDimension(topElement.GetComponent<MeshRenderer>().bounds) * 3;
        }
        private void SetItemIdleSize()
        {
            var idleSize = MathExtended.GetPolygonSideSizeByCircle(
                sr.Cs.radius, 
                carouselItems.Length) / Mathf.Sqrt(3);
            sr.Cs.itemIdleSize = new Vector3(idleSize, idleSize ,idleSize);
        }

        private void SetSelectedPoint()
        {
            var trn = transform;
            sr.Cs.selectedPoint =
                trn.position +
                trn.forward *
                (sr.Cs.radius + sr.Cs.itemSelectedSize.x);
        }
        
        private void SetItemSelectedSize()
        {
            var res = sr.Cs.itemIdleSize * sr.Cs.idleToSelectedRelation;
            sr.Cs.itemSelectedSize = res;
        }
        
        private void ArrangeInCircle()
        {
            var mySequence = DOTween.Sequence();
            var radius = sr.Cs.radius;
            
            // we assume that cubes have same sizes
            for (var index = 0; index < carouselItems.Length; index++)
            {
                var currIx = index;
                ArrangementHelper.GetPositionInCircle(
                    transform.position, 
                    currIx,
                    carouselItems.Length,
                    //carouselItems.Length + sr.Cs.idleToSelectedRelation - 1,
                    //sr.Cs.idleToSelectedRelation,
                    radius,
                    (pos, rot) =>
                    {
                        carouselItems[currIx].initLocalPosition = pos;
                        
                        mySequence
                            .Insert(0, carouselItems[currIx]
                                .transform.DOMove(pos, 1))
                            .Insert(0, carouselItems[currIx].transform
                                .DORotateQuaternion(rot, 1))
                            .Insert(0, carouselItems[currIx].transform
                                .DOScale(sr.Cs.itemIdleSize, 1));
                        mySequence.PlayForward();
                    }
                );
            }

            SetSelectedPoint();
            SetDeltaAngle(360f / (carouselItems.Length + sr.Cs.idleToSelectedRelation - 1));
        }

    }
}