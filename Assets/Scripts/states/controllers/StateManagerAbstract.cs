using states.cubes;
using UnityEngine;

namespace states.controllers
{

    public abstract class StateManagerAbstract : MonoBehaviour, IStateManager
    {
        private CubeBaseState initialState;
        private CubeBaseState CurrentState { get; set; }

        protected void TransitionToState(CubeBaseState state)
        {
            CurrentState?.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this); 
        }

        protected void SetInitialState(CubeBaseState initState)
        {
            initialState = initState;
        }

        private void Awake()
        {
            if (initialState != null)
            {
                TransitionToState(initialState);
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