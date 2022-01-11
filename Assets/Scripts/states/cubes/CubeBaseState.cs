using cubes;
using input.slidermenu.controllers;
using Lean.Common;
using states.controllers;
using UnityEngine;

namespace states.cubes
{
    public abstract class CubeBaseState
    {
        protected Transform transform;
        protected CubeController cubeController;
        //protected SliderTopController parentController;

        public virtual void EnterState(IStateManager stateManager)
        {
            transform = ((StateManagerAbstract) stateManager).transform;
            cubeController = transform.GetComponent<CubeController>();
            // Debug.Log($"{transform.name} entered {GetType().Name}");
            // parentController = transform.GetComponentInParent<SliderTopController>();
        }

        public virtual void ExitState(IStateManager stateManager)
        {
            /*_sp = stateManager.GetTransformable();
            Debug.Log($"ExitState: {GetType().Name}, transform: {GetSparePart()}");*/
        }
        
        public abstract void LogicUpdate(IStateManager stateManager);
        
        public abstract void PhysicsUpdate(IStateManager stateManager);
            
    }
    
}