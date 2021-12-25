using System;
using System.Collections.Generic;
using events;
using Lean.Common;
using Lean.Touch;
using leanExtended;
using UnityEngine;

namespace scroll.scroll.swipe
{
    // [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanManualTranslate))]
    [RequireComponent(typeof(LeanSelectable))]
    public class CubeCtr : MonoBehaviour
    {
        // private LeanMultiUpdate _lmu;
        private LeanMultiDirectionExtended _lmuDragAside, _lmuSwipeUp;
        private LeanManualTranslate _ldm;
        private SwpMgr _parentMgrCached;
        public CubeCtr prev;
        public CubeCtr next;
        public CubeCtr followTo;

        public float offset;
        public bool head;

        private MeshRenderer _mr;

        private void OnEnable()
        {

            _ldm = GetComponent<LeanManualTranslate>();

            _mr = GetComponent<MeshRenderer>();
            
            _ldm.Multiplier = 0.01f;
            _ldm.DirectionB = Vector3.zero;
            
            SwipeMenuEvents.Current.OnHeadChanged += ProcessNewHead;
            SwipeMenuEvents.Current.OnSwipeUp += CheckSelected;
        }

        public void SetComponentsFromParent()
        {
            _parentMgrCached = GetComponentInParent<SwpMgr>();
            _lmuSwipeUp = _parentMgrCached.swipeUpLmu;
            _lmuDragAside = _parentMgrCached.dragLmu;
        }

        private void OnDisable()
        {
            try
            {
                SwipeMenuEvents.Current.OnHeadChanged -= ProcessNewHead;
                SwipeMenuEvents.Current.OnFingerDown -= _lmuDragAside.AddFinger;
                SwipeMenuEvents.Current.OnSwipeUp -= CheckSelected;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        private void OnDestroy()
        {
            try
            {
                _lmuDragAside.OnDelta.RemoveAllListeners();
            }
            catch (Exception)
            {
                // ignore
            }
        }


        /*private void HandleSwipeUp(List<LeanFinger> fingers, float delta)
        {
            // _swiped = true;
            var mid = _parentMgrCached.cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
            // transform.DOMove(mid, 1);
        }*/

        private void CheckSelected(Vector3 swipePos)
        {
            if (_mr.bounds.Contains(swipePos))
                Debug.Log($"i'm selected - {transform.name}");
        }
        
        
        private void HandleDragAside(List<LeanFinger> fingers, float delta)
        {
            _ldm.TranslateA(delta);
        }


        private void ProcessNewHead(CubeCtr newHead)
        {
            if (newHead.Equals(this))
            {
                transform.localPosition = _parentMgrCached.startPos;
                head = true;
                name += "_head";
                followTo = null;
                _mr.material.color = Color.red;
                _lmuDragAside.OnDelta.AddListener(HandleDragAside);
            }
            else
            {
                head = false;
                followTo = newHead;
                CalculateOffset(followTo);
                _lmuDragAside.OnDelta.RemoveListener(HandleDragAside);
                name = name.Replace("_head", "");
                _mr.material.color = Color.blue;
            }
        }

        private void CalculateOffset(CubeCtr newHead)
        {
            offset = newHead.transform.localPosition.x - transform.localPosition.x;
        }


        
        
        private bool CheckVisible()
        {
            return Screen.safeArea.Contains(_parentMgrCached.cam.WorldToScreenPoint(transform.position));
        }
        

        // Update is called once per frame
        void Update()
        {
            try
            {
                if (head) return;
                transform.localPosition = followTo.transform.localPosition + Vector3.left * offset;
                if (!CheckVisible())
                    gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.LogError($"error in {name}: {e}");
            }
        }
    }
}
