using System;
using helpers;
using models;
using models.sparepart.iteration2;
using UnityEngine;

namespace states.controllers
{
    public abstract class BaseState
    {
        private TransformableAbstract _sp;
        private SingleQueueExecutor _sqe;

        protected TransformableAbstract GetSparePart()
        {
            return _sp;
        }

        protected SingleQueueExecutor GetExecutor()
        {
            return _sqe;
        }
        
        public virtual void EnterState(IStateManager stateManager)
        {
            // _sp = stateManager.GetTransformable();
            _sqe = new SingleQueueExecutor(stateManager.AttachedTo());
            Debug.Log($"EnterState: {GetType().Name}, transform: {GetSparePart()}");
        }

        public virtual void ExitState(IStateManager stateManager)
        {
            _sp = stateManager.GetTransformable();
            Debug.Log($"ExitState: {GetType().Name}, transform: {GetSparePart()}");
        }
        
        public abstract void LogicUpdate(IStateManager stateManager);
        public abstract void PhysicsUpdate(IStateManager stateManager);
            
    }
    
}