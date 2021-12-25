using Lean.Common;
using scroll.scroll.swipe;
using UnityEngine;

namespace states.cubes
{
    public abstract class BaseState
    {
        /*private TransformableAbstract _sp;
        private SingleQueueExecutor _sqe;

        protected TransformableAbstract GetSparePart()
        {
            return _sp;
        }

        protected SingleQueueExecutor GetExecutor()
        {
            return _sqe;
        }*/
        
        private LeanManualTranslate _ldm;
        protected CubeCtr prev;
        protected CubeCtr next;
        protected MeshRenderer _mrCached;
        protected Transform transform;

        public virtual void EnterState(IStateManager stateManager)
        {
            var sm = (StateManagerAbstract) stateManager;
            transform = sm.transform;
            _mrCached = sm.transform.GetComponent<MeshRenderer>();
            /*_sp = stateManager.GetTransformable();
            _sqe = new SingleQueueExecutor(stateManager.AttachedTo());
            Debug.Log($"EnterState: {GetType().Name}, transform: {GetSparePart()}");*/
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