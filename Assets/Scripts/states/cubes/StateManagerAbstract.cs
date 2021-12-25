using UnityEngine;

namespace states.cubes
{

    public abstract class StateManagerAbstract : MonoBehaviour, IStateManager
    {
        private BaseState _initialState;
        private BaseState CurrentState { get; set; }

        protected void TransitionToState(BaseState state)
        {
            CurrentState?.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this); 
        }

        protected void SetInitialState(BaseState initState)
        {
            _initialState = initState;
        }

        public MonoBehaviour AttachedTo()
        {
            return this;
        }

        private void Awake()
        {
            if (_initialState != null)
            {
                TransitionToState(_initialState);
            }
        }
        
        private void Update()
        {
            CurrentState.LogicUpdate(this);
        }

        private void FixedUpdate()
        {
            CurrentState.PhysicsUpdate(this);
        }

    }
}