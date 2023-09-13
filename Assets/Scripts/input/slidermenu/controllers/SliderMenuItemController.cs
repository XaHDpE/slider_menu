using System;
using Lean.Common;
using models.cube;
using states.controllers;
using UnityEngine;

namespace input.slidermenu.controllers
{
    [RequireComponent(typeof(LeanManualTranslate))]
    [RequireComponent(typeof(CubeStateManager))]
    [Serializable]
    public class SliderMenuItemController : MonoBehaviour
    {
        private LeanManualTranslate mt;

        [SerializeField]
        public SliderMenuItem model;
        
        private CubeStateManager csm;

        private void OnEnable()
        {
            mt = GetComponent<LeanManualTranslate>();
            csm = GetComponent<CubeStateManager>();
            SetupManualTranslate(ref mt);
        }

        private void SetupManualTranslate(ref LeanManualTranslate mt1)
        {
            mt1.Multiplier = 0.01f;
            mt1.DirectionB = Vector3.zero;
        }

        public void MoveSmiToActive()
        {
            csm.MoveToActive();
        }

        public void MoveSmiToInactive()
        {
            csm.MoveToIdle();
        }

        public void MoveSmiToHead()
        {
            csm.MoveToHead();
        }
        
    }
}