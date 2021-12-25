using System;
using UnityEngine;

namespace states.cubes
{
    public class CubeIdleState : BaseState
    {
        public override void EnterState(IStateManager stateManager)
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
        }
        
        public override void ExitState(IStateManager stateManager)
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
        }

        public override void LogicUpdate(IStateManager stateManager)
        {
            
        }

        public override void PhysicsUpdate(IStateManager stateManager)
        {
            
        }
    }
}