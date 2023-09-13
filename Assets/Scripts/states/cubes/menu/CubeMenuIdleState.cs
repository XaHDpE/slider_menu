using states.controllers;
using UnityEngine;

namespace states.cubes.menu
{
    public class CubeMenuIdleState : CubeMenuBaseState
    {
        public override void EnterState(IStateManager stateManager)
        {
            base.EnterState(stateManager);
            transform.gameObject.SetActive(false);
            /*if (transform.TryGetComponent(out MeshRenderer mr))
                mr.enabled = false;*/
        }
        
        public override void ExitState(IStateManager stateManager)
        {
            /*var trn = ((CubeStateManager) stateManager).transform; 
            trn.GetComponent<MeshRenderer>().enabled = true;*/
            transform.gameObject.SetActive(true);
        }

        public override void LogicUpdate(IStateManager stateManager)
        {
            
        }

        public override void PhysicsUpdate(IStateManager stateManager)
        {
            
        }
    }
}