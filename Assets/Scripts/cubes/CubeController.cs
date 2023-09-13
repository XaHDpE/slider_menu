using states.controllers;
using UnityEngine;

namespace cubes
{

    [RequireComponent(typeof(CubeStateManager))]
    [RequireComponent(typeof(BoxCollider))]
    public class CubeController : MonoBehaviour
    {
        private CubeStateManager stateManager;

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