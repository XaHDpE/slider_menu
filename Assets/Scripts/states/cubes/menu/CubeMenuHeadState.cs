using System.Collections.Generic;
using DG.Tweening;
using input.slidermenu.controllers;
using Lean.Common;
using Lean.Touch;
using settings;
using states.controllers;
using UnityEngine;

namespace states.cubes.menu
{
    public class CubeMenuHeadState : CubeMenuBaseState
    {
        private LeanManualTranslate lmt;
        private SliderTopController sliderTop;
        private MeshRenderer mr;
        private float distanceTravelled, scaleX;
        private Vector3 lastPosition;
        private bool precisingPositionInProgress;

        public override void EnterState(IStateManager stateManager)
        {
            base.EnterState(stateManager);
            scaleX = parentController.ViewManager.GetItemSize();
            transform.name += "_head";
            mr = transform.GetComponent<MeshRenderer>();
            // mr.material.color = Color.red;
            lmt = ((CubeStateManager) stateManager).GetComponent<LeanManualTranslate>();
            lmt.Multiplier = SettingsReader.Instance.sliderMenuSettings.swipeSpeed;
            lmt.DirectionA = parentController.ViewManager.GetSlideDirection();
            lmt.DirectionB = Vector3.zero;
            sliderTop = transform.GetComponentInParent<SliderTopController>();
            sliderTop.dragLmu.OnDelta.AddListener(HandleDragAside);
            lastPosition = transform.localPosition;
            // LeanTouch.OnFingerUp += Test;
        }

        private void HandleDragAside(List<LeanFinger> fingers, float delta)
        {
            var reducedDelta = delta;
            // Debug.Log($"delta: {delta}, distance: {delta * lmt.Multiplier}");
            lmt.TranslateA(reducedDelta);
        }
        
        public override void ExitState(IStateManager stateManager)
        {
            base.ExitState(stateManager);
            transform.name = transform.name.Replace("_head", "");
            // mr.material.color = Color.gray;
            sliderTop.dragLmu.OnDelta.RemoveListener(HandleDragAside);
            // LeanTouch.OnFingerUp -= Test;
        }

        public override void LogicUpdate(IStateManager stateManager)
        {
            /*if (precisingPositionInProgress) return;
            distanceTravelled += transform.localPosition.x - lastPosition.x;
            if (transform.localPosition.Equals(lastPosition) && Mathf.Abs(distanceTravelled) > 0)
            { 
                MoveToClosestValidPoint(distanceTravelled, () =>
                {
                    distanceTravelled = 0; 
                });
            }
            
            lastPosition = transform.localPosition;*/
        }

        private void MoveToClosestValidPoint(float distancePassed, TweenCallback callback)
        {
            // Debug.Log($"distancePassed: {distancePassed}");
            precisingPositionInProgress = true;
            var delta = Mathf.Abs(distancePassed) < scaleX ? distancePassed : distancePassed%scaleX;
            var deltaV = transform.localPosition + Vector3.left * delta;
            precisingPositionInProgress = false;
            transform.DOMove(deltaV, 0.1f).OnComplete(callback);
        }


        public override void PhysicsUpdate(IStateManager stateManager)
        {
            
        }
        
    }
}