using System;
using DG.Tweening;
using states.controllers;
using UnityEngine;
using Th = helpers.TransformHelper;

namespace states.sparepart
{
    public class SparePartDetachedState : BaseState
    {
        public override void EnterState(IStateManager stateManager)
        {
            var mapController = GameObject.FindObjectOfType<MapController>();
            var pos = Camera.main.transform.position;
            var cube = stateManager.GetTransformable().GetComponent<CubeController>();
            
            Array.Sort(mapController.positions,
                (v1, v2) => 
                    Vector3.Distance(v1, pos).CompareTo(Vector3.Distance(v2, pos)));
            
            Debug.DrawRay(mapController.positions[0], Vector3.forward * 50, Color.blue, 100);

            DOTween.Sequence()
                .Insert(0,
                    stateManager.GetTransformable().transform.DOMove(mapController.positions[mapController.positions.Length-1], 1))
                .Insert(0,
                    stateManager.GetTransformable().transform.DOScale(cube.initialScale, 1)
                    )
                .PlayForward();

            // base.EnterState(stateManager);
            // jump to first available placeholder
        }

        public override void ExitState(IStateManager stateManager)
        {
            base.ExitState(stateManager);
        }

        public override void LogicUpdate(IStateManager stateManager) {}

        public override void PhysicsUpdate(IStateManager stateManager) {}
        
    }
}