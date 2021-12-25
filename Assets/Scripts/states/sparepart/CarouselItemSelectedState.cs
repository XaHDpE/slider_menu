using System;
using DG.Tweening;
using helpers;
using models.sparepart;
using settings;
using states.controllers;
using UnityEngine;

namespace states.sparepart
{
    [Serializable]
    public class CarouselItemSelectedState : BaseState
    {

        private float _boundsSize;
        private float _rotationSpeed = 15;
        private SparePartStateManager _cism;
        
        
        // events and delegates
        public delegate void NewElementInTopBarDelegate(SparePartStateManager target);
        public static event NewElementInTopBarDelegate NewIsMovingInTopBar;
        
        public override void EnterState(IStateManager stateManager)
        {
            // base.EnterState(stateManager);
            
            _cism = stateManager as SparePartStateManager;
            var trn = stateManager.GetTransformable();

            var s1 = DOTween.Sequence() 
                /*.Insert(0,
                    trn.transform.DOMove(
                        SettingsReader.Instance.carouselSettings.selectedPoint,
                        SettingsReader.Instance.carouselSettings.Selected2IdleChangeTime
                    )
                )*/
                .Insert(0,
                    trn.transform.DOScale(
                        SettingsReader.Instance.carouselSettings.itemSelectedSize,
                        SettingsReader.Instance.carouselSettings.Selected2IdleChangeTime
                    )
                ); 
            s1.SetAutoKill(true);
            s1.PlayForward();
            
            trn.name += " - selected";
            
            // register events
            Swipable.SwipeUp += MoveToDetached;
        }

        public override void ExitState(IStateManager stateManager)
        {
            // base.ExitState(stateManager);


            stateManager.GetTransformable().name = stateManager.GetTransformable().name.Replace(" - selected", "");
            
            // unregister events
            // Carousel5.NewElementCameToCenter -= EnableSwipe;
            // Swipable.SwipeUp -= SwipeToTopBar;
            Swipable.SwipeUp -= MoveToDetached;
        }
        
        // base monoBehavior methods   
        public override void LogicUpdate(IStateManager stateManager)
        {
            
        }

        public override void PhysicsUpdate(IStateManager stateManager)
        {
            stateManager.GetTransformable().Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }


        private void MoveToDetached(Vector2 delta)
        {
            Debug.Log("moving to detached");
            _cism.MoveToDetached();
        }

    }

}