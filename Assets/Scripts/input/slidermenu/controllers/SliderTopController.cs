using System;
using cubes;
using DG.Tweening;
using events;
using input.slidermenu.helpers;
using leanExtended;
using settings;
using UnityEditor;
using UnityEngine;

namespace input.slidermenu.controllers
{
    public class SliderTopController : MonoBehaviour
    {
        public Camera cam;
        public Vector3 startPos, startScale, prevHeadPos;
        public SliderMenuItemController newest, head;

        public LeanMultiDirectionExtended dragLmu;
        public LeanMultiDirectionExtended swipeUpLmu;
        
        private bool initDone;
        
        private int i;
        public float curDistance, curOffset, startBoundsX;
        
        private SliderMenuItemController SetHead(SliderMenuItemController newHead)
        {
            var headCandidate = newHead;
            headCandidate.MoveToHead();
            SwipeMenuEvents.Current.HeadChanged(headCandidate);
            return headCandidate;
        }

        public void Init(SliderMenuItemController[] cubes, int headIdx)
        {
            startPos = SliderMenuHelper.SetStartPoint(cam, 0);
            
            Handles.SphereHandleCap(1, startPos, Quaternion.identity, 1, EventType.Ignore);
            
            newest = cubes[headIdx];
            var newestTrn = newest.transform;
            startScale = newestTrn.localScale;
            startBoundsX = newestTrn.GetComponent<Renderer>().bounds.size.x;
            newestTrn.localPosition = startPos;
            head = SetHead(newest);
            initDone = true;
            head.transform.DOMove(
                cam.ViewportToWorldPoint(SettingsReader.Instance.sliderMenuSettings.eastSpawnPoint),
                1f
            );

        }

        private SliderMenuItemController SpawnActiveOnStartPoint(SliderMenuItemController obj)
        {
            obj.transform.localPosition = startPos;
            obj.MoveToActive();
            return obj;
        }

        private void Update()
        {
            if (!initDone) return;

            curDistance = (newest.transform.localPosition - startPos).x;
            // Debug.Log($"curDistance: {curDistance}");
            if (Mathf.Abs(curDistance) >= startScale.magnitude)
            {
                startPos = SliderMenuHelper.SetStartPoint(cam, curDistance);
                curOffset = Math.Sign(curDistance) * startScale.magnitude * i++;
                newest = SpawnActiveOnStartPoint(curDistance.Equals(Mathf.Abs(curDistance)) ? newest.next : newest.prev);
            }

            if (Vector3.Distance(head.transform.localPosition, prevHeadPos) < 0.001f && !newest.Equals(head))
            {
                head.MoveToActive();
                head = SetHead(newest);
                i = 0;
            }
            
            prevHeadPos = head.transform.localPosition;
        }
        
    }
}
