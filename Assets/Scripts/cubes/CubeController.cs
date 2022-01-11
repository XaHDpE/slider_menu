using input.slidermenu.models;
using states.controllers;
using UnityEngine;

namespace cubes
{

    [RequireComponent(typeof(CubeStateManager))]
    public class CubeController : MonoBehaviour
    {
        public CubeStateManager stateManager;
        public CubeLight model;
        
        private void Awake()
        {
            if (TryGetComponent(out CubeStateManager stMgr))
            {
                stateManager = stMgr;
                // Debug.Log($"CubeStateManager is assigned to the {transform}");
            }
            else
            {
                Debug.LogError($"Something wrong with CubeController for {transform}");
            }
        }
        
    }
}