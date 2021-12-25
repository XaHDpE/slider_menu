using DG.Tweening;
using settings;
using states.controllers;
using Th = helpers.TransformHelper;

namespace states.sparepart
{
    public class CarouselItemIdleState : BaseState
    {
        public override void EnterState(IStateManager stateManager)
        {
            // base.EnterState(stateManager);
            var cism = stateManager as SparePartStateManager;
            
            var trn = stateManager.GetTransformable();
            
            var mySeq = DOTween.Sequence() 
                /*.Insert(0,
                    trn.transform.DOLocalMove(
                        cism.initLocalPosition,
                        SettingsReader.Instance.carouselSettings.Selected2IdleChangeTime
                    )
                )*/
                .Insert(0,
                    trn.transform.DOScale(
                        SettingsReader.Instance.carouselSettings.itemIdleSize,
                        SettingsReader.Instance.carouselSettings.Selected2IdleChangeTime
                    )
                );
            mySeq.SetAutoKill(true);
            mySeq.PlayForward();
        }

        public override void LogicUpdate(IStateManager stateManager) {}

        public override void PhysicsUpdate(IStateManager stateManager) {}
        
    }
}