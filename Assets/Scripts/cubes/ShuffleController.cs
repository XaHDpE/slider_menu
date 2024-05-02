using System;
using events;
using helpers;
using settings;
using slicing.controllers;
using UnityEngine;

namespace cubes
{
    public class ShuffleController : MonoBehaviour
    {

        private Transform targetTop;
        public Vector3[] positions;
    
        private void Start()
        {
            CustomGameEvents.Current.OnObjectSliced += Execute;
        }

        private void OnDestroy()
        {
            CustomGameEvents.Current.OnObjectSliced -= Execute;
        }

        private void Execute(Transform parentObj, Vector3Int counts)
        {
            targetTop = parentObj;
        
            var cubes = targetTop.GetComponentsInChildren<CubeController>();
            positions = new Vector3[cubes.Length];

            foreach (var cube in cubes)
            {
                var curRotation = MathExtended.GetRandomRotation();
                if (cube.transform.childCount <= 0)
                    cube.gameObject.SetActive(false);
                cube.transform.Rotate(curRotation.x, curRotation.y, curRotation.z);
            }
            
            CustomGameEvents.Current.ShuffleDone(transform);
            
        }
        
    }
}