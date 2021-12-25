using models;
using models.sparepart.iteration2;
using UnityEngine;

namespace states.controllers
{
    [RequireComponent(typeof(Transformable))]
    public abstract class StateManagerAbstract : MonoBehaviour, IStateManager
    {
        private BaseState _initialState;
        [SerializeField] private BaseState CurrentState { get; set; }

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


        public TransformableAbstract GetTransformable()
        {
            if (!TryGetComponent<TransformableAbstract>(out var res))
                Debug.LogError("unable to GetTransformable");
            return res;
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