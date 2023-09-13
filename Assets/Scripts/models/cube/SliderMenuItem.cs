using System;
using cubes;
using input.slidermenu.controllers;

namespace models.cube
{
    [Serializable]
    public class SliderMenuItem
    {
        public SliderMenuItemController next;
        public SliderMenuItemController prev;
        public SliderMenuItemController followTo;
    }
}