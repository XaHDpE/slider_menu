using System;
using DG.Tweening;
using events;
using leanExtended;
using UnityEngine;

namespace scroll.scroll.swipe
{
    public class SwpMgr : MonoBehaviour
    {
        public Camera cam;
        public Vector3 startPos, startScale, prevPos;
        public CubeCtr newest, head;

        public LeanMultiDirectionExtended dragLmu;
        public LeanMultiDirectionExtended swipeUpLmu;
        
        private CubeCtr[] _items;
        private bool _initDone;

        private void Start()
        {
            const int maxVisible = 11;
            _items = SwpDataProvider.Fill(transform, 50, maxVisible);
            startPos = cam.ViewportToWorldPoint(new Vector3(.1f, .1f, 10));
            startScale = _items[0].transform.localScale;
            prevPos = startPos;
            newest = Spawn(_items[0], true, 0);
            SetHead();
            _initDone = true;
            InitPlace(maxVisible);
        }

        private void InitPlace(int maxCount)
        {
            var finalDest = startPos + Vector3.right * startScale.magnitude * maxCount;
            var trn1 = head.transform;
            trn1.DOMove(finalDest, 1);
        }

        private CubeCtr Spawn(CubeCtr cc, bool isHead, float curOffset)
        {
            cc.SetComponentsFromParent();
            cc.gameObject.SetActive(true);
            cc.transform.localPosition = startPos;
            cc.head = isHead;
            if (!isHead)
            {
                cc.followTo = head;
                cc.offset = curOffset;
            }

            return cc;
        }
        
        private void SetHead()
        {
            head = newest;
            SwipeMenuEvents.Current.HeadChanged(head);
        }

        // Update is called once per frame

        private int _i;
        private float _curDistance;

        private void SetStartPoint(float delta)
        {
            startPos = Mathf.Abs(delta).Equals(delta)
                ? cam.ViewportToWorldPoint(new Vector3(.1f, .1f, 10))
                : cam.ViewportToWorldPoint(new Vector3(.9f, .1f, 10));
        }

        private void Update()
        {
            if (!_initDone) return;

            _curDistance = (newest.transform.localPosition - startPos).x;

            if (Mathf.Abs(_curDistance) >= startScale.magnitude)
            {
                SetStartPoint(_curDistance);
                newest = Spawn(
                    _curDistance.Equals(Mathf.Abs(_curDistance)) ? newest.next : newest.prev,
                    false,
                    Math.Sign(_curDistance) * startScale.magnitude * _i++
                );
            }

            if (Vector3.Distance(head.transform.localPosition, prevPos) < 0.001f && !newest.Equals(head))
            {
                SetHead();
                _i = 0;
            }

            prevPos = head.transform.localPosition;
        }
    }
}