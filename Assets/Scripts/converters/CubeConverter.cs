using System;
using cubes;
using input.slidermenu.controllers;
using UnityEngine;

namespace converters
{
    public static class CubeConverter
    {
        public static SliderMenuItemController Convert(CubeController tr)
        {
            return tr.gameObject.AddComponent<SliderMenuItemController>();
        }
    }
}