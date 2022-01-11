using input.slidermenu.controllers;

namespace states.cubes.menu
{
    public class CubeMenuBaseState : CubeBaseState
    {
        
        protected SliderTopController parentController;
        
        public override void EnterState(IStateManager stateManager)
        {
            base.EnterState(stateManager);
            parentController = transform.GetComponentInParent<SliderTopController>();
        }

        public override void LogicUpdate(IStateManager stateManager)
        {
            
        }

        public override void PhysicsUpdate(IStateManager stateManager)
        {
        }
    }
}