using System;
using cubes;
using input.slidermenu.controllers;
using UnityEngine;

namespace input.slidermenu.models
{
    [Serializable]
    public class CubeLight
    {
        public CubeController next;
        public CubeController prev;
        public CubeController followTo;
        
        public Vector3 initialLocalPosition;
        public Vector3 initialScale;
        public Quaternion initialRotation;
        
    }
}