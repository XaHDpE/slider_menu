using System;
using Lean.Common;
using states.controllers;
using UnityEngine;

namespace input.slidermenu.controllers
{
    [RequireComponent(typeof(LeanManualTranslate))]
    [RequireComponent(typeof(CubeStateManager))]
    public class SliderMenuItemController : MonoBehaviour
    {
        private LeanManualTranslate manualTranslate;

        public SliderMenuItemController next;
        public SliderMenuItemController prev;
        private CubeStateManager _csm;

        private void OnEnable()
        {
            manualTranslate = GetComponent<LeanManualTranslate>();
            _csm = GetComponent<CubeStateManager>();
        }

        public void MoveToActive()
        {
            _csm.MoveToActive();
        }

        public void MoveToInactive()
        {
            _csm.MoveToIdle();
        }

        public void MoveToHead()
        {
            _csm.MoveToHead();
        }
        
    }
}