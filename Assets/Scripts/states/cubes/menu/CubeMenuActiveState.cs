using System;
using DG.Tweening;
using events;
using helpers;
using input.slidermenu.controllers;
using states.controllers;
using UnityEngine;

namespace states.cubes.menu
{
    public class CubeMenuActiveState : CubeMenuBaseState
    {
        private float offset;
        private Transform followTo;
 
        public override void EnterState(IStateManager stateManager)
        {
            base.EnterState(stateManager);
            transform.name += "_active";
            followTo = parentController.head.transform;
            // offset = TransformHelper.CalculateOffset(transform, followTo);
            offset = parentController.curOffset;
            SwipeMenuEvents.Current.OnHeadChanged += ProcessNewHead;
            SwipeMenuEvents.Current.OnSwipeUp += CheckSwipedUp;
        }

        private void CheckSwipedUp(Vector3 pos)
        {
            var bounds = transform.GetComponent<MeshRenderer>().bounds;
            if (bounds.Contains(pos))
            {
                Debug.Log($"{transform.name} is guilty!");
                transform.DOMove(
                    parentController.ViewManager.menuCam.cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 15)),
                    1
                );
                
            }
        }
        
        
        public override void ExitState(IStateManager stateManager)
        {
            SwipeMenuEvents.Current.OnHeadChanged -= ProcessNewHead;
            transform.name = transform.name.Replace("_active", "");
            SwipeMenuEvents.Current.OnSwipeUp -= CheckSwipedUp;
        }

        private void ProcessNewHead(SliderMenuItemController newHead)
        {
            followTo = newHead.transform;
            offset = TransformHelper.CalculateOffset(followTo, transform);
            // Debug.Log($"calculated offset between new head {newHead.name} and {transform.name} is {offset}");
        }

        public override void LogicUpdate(IStateManager stateManager)
        {
            try
            {
                transform.localPosition = followTo.localPosition + Vector3.left * offset;
                if (!ScreenHelper.CheckVisible(parentController.ViewManager.menuCam.cam, transform.position))
                    ((CubeStateManager) stateManager).MoveToIdle();
            }
            catch (Exception e)
            {
                Debug.LogError($"error in {transform.name}: {e}");
            }
        }

        public override void PhysicsUpdate(IStateManager stateManager)
        {
            
        }
    }
}