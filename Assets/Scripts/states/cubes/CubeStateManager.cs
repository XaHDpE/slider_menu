namespace states.cubes
{
    
    public class CubeStateManager :  StateManagerAbstract
    {

        private readonly CubeSelectedState _cubeSelectedState = new CubeSelectedState();
        private readonly CubeIdleState _spIdleState = new CubeIdleState();

        public CubeStateManager()
        {
            SetInitialState(_spIdleState);
        }

        public void MoveToSelectedInList()
        {
            TransitionToState(_cubeSelectedState);
        }

        public void MoveToIdle()
        {
            TransitionToState(_spIdleState);
        }

    }
}