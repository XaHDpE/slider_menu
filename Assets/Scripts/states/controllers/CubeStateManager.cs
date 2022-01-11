using states.cubes;
using states.cubes.menu;

namespace states.controllers
{
    public class CubeStateManager :  StateManagerAbstract
    {
        private readonly CubeDefaultState cubeDefaultState = new CubeDefaultState();
        private readonly CubeMenuHeadState cubeMenuHeadState = new CubeMenuHeadState();
        private readonly CubeMenuIdleState spMenuIdleState = new CubeMenuIdleState();
        private readonly CubeMenuActiveState spMenuActiveState = new CubeMenuActiveState();

        public CubeStateManager()
        {
            SetInitialState(cubeDefaultState);
        }

        public void MoveToHead()
        {
            TransitionToState(cubeMenuHeadState);
        }

        public void MoveToIdle()
        {
            TransitionToState(spMenuIdleState);
        }

        public void MoveToActive()
        {
            TransitionToState(spMenuActiveState);
        }

    }
}