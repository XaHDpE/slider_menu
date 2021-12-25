using states.sparepart;
using UnityEngine;

namespace states.controllers
{
    
    public class SparePartStateManager :  StateManagerAbstract
    {

        private readonly CarouselItemSelectedState _spSelectedInCarouselState = new CarouselItemSelectedState();
        private readonly CarouselItemIdleState _spIdleState = new CarouselItemIdleState();
        private readonly SparePartDetachedState _spDetachedState = new SparePartDetachedState(); 

        public Vector3 initLocalPosition;

        public SparePartStateManager()
        {
            SetInitialState(_spIdleState);
        }

        public void MoveToSelectedInList()
        {
            TransitionToState(_spSelectedInCarouselState);
        }

        public void MoveToIdle()
        {
            TransitionToState(_spIdleState);
        }

        public void MoveToDetached()
        {
            TransitionToState(_spDetachedState);
        }
        
    }
}