using System;
using DG.Tweening;
using events;
using input.slidermenu.providers;
using input.slidermenu.view;
using leanExtended;
using UnityEngine;

namespace input.slidermenu.controllers
{
    public class SliderTopController : MonoBehaviour
    {
        public RectTransform menuCanvas;
        public Vector3 startPos, prevHeadPos, startScale;
        public SliderMenuItemController newest, head;
        public LeanMultiDirectionExtended dragLmu;

        private bool initDone;

        private SlideMenuViewManager viewManager;
        
        private int i;
        public float curDistance, curOffset;
        private SliderMenuItemController[] items;

        public SlideMenuViewManager ViewManager => viewManager;

        public void Init(SlideMenuViewManager vm, SliderDataProvider sdp)
        {
            viewManager = vm;
            startScale = viewManager.GetStartScale();
            viewManager.RescaleCanvas(menuCanvas);
            items = sdp.Items;
            newest = items[0];
            viewManager.SpawnAtStart(newest);
            head = SetHead(newest);
            initDone = true;
            //head.transform.DOMove(viewManager.GetWestPoint(), 1f);
        }
        
        private SliderMenuItemController SetHead(SliderMenuItemController newHead)
        {
            var headCandidate = newHead;
            headCandidate.MoveSmiToHead();
            SwipeMenuEvents.Current.HeadChanged(headCandidate);
            return headCandidate;
        }

        private void Update()
        {
            if (!initDone) return;

            curDistance = (newest.transform.position - viewManager.GetSpawnPointPosition()).x;
            
            if (Mathf.Abs(curDistance) >= viewManager.GetStartScale().magnitude)
            {
                viewManager.SetSpawnPoint(curDistance);
                curOffset = Math.Sign(curDistance) * viewManager.GetStartScale().magnitude * i++;
                newest = ViewManager.SpawnAtStart(
                    curDistance.Equals(Mathf.Abs(curDistance)) ? newest.model.next : newest.model.prev
                    );
                newest.MoveSmiToActive();
            }

            if (Vector3.Distance(head.transform.position, prevHeadPos) < 0.01f && !newest.Equals(head))
            {
                head.MoveSmiToActive();
                head = SetHead(newest);
                i = 0;
            }
            
            prevHeadPos = head.transform.position;
        }
        
    }
}