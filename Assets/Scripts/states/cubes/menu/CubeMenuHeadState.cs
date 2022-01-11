using System.Collections.Generic;
using DG.Tweening;
using input.slidermenu.controllers;
using Lean.Common;
using Lean.Touch;
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
            scaleX = parentController.startBoundsX;
            transform.name += "_head";
            mr = transform.GetComponent<MeshRenderer>();
            mr.material.color = Color.red;
            lmt = ((CubeStateManager) stateManager).GetComponent<LeanManualTranslate>();
            lmt.Multiplier = 0.01f;
            lmt.DirectionB = Vector3.zero;
            sliderTop = transform.GetComponentInParent<SliderTopController>();
            sliderTop.dragLmu.OnDelta.AddListener(HandleDragAside);
            
            lastPosition = transform.localPosition;
            
        }

        private void HandleDragAside(List<LeanFinger> fingers, float delta)
        {
            lmt.TranslateA(delta);
        }
        
        public override void ExitState(IStateManager stateManager)
        {
            base.ExitState(stateManager);
            transform.name = transform.name.Replace("_head", "");
            mr.material.color = Color.gray;
            sliderTop.dragLmu.OnDelta.RemoveListener(HandleDragAside);
        }

        public override void LogicUpdate(IStateManager stateManager)
        {

            if (precisingPositionInProgress) return;
            
            distanceTravelled += transform.localPosition.x - lastPosition.x;
            if (transform.localPosition.Equals(lastPosition))
            { 
                if (Mathf.Abs(distanceTravelled) > 0) {
                    MoveToClosestValidPoint(distanceTravelled, () =>
                    {
                        distanceTravelled = 0;
                    });
                }
            }
            
            lastPosition = transform.localPosition;
        }

        private void MoveToClosestValidPoint(float distancePassed, TweenCallback callback)
        {
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