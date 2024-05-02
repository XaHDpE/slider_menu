using cubes;
using events;
using settings;
using slicing.controllers;
using UnityEngine;

namespace map
{
    public class MapController : MonoBehaviour
    {

        private Transform targetTop;
        public Vector3[] positions;
    
        private void Start()
        {
            // CustomGameEvents.Current.OnObjectSliced += CreateMap;
        }

        private void OnDestroy()
        {
            // CustomGameEvents.Current.OnObjectSliced -= CreateMap;
        }

        private void CreateMap(Transform parentObj, Vector3Int counts)
        {
            targetTop = parentObj;
        
            var cubes = targetTop.GetComponentsInChildren<CubeController>();
            positions = new Vector3[cubes.Length];

            for (var i = 0; i < cubes.Length; i++)
            {
                var cube = cubes[i];
                
                if (cube.transform.childCount <= 0) continue;
                
                Debug.Log($"cube: {cube.InFront()}");
                
                var sliceItemModel = cube.GetComponentInChildren<SliceItemController>().model;
                positions[i] = sliceItemModel.initialLocalPosition;
                
                var shell = Instantiate(
                    SettingsReader.Gs.shellPrefab, 
                    sliceItemModel.initialLocalPosition, 
                    sliceItemModel.initialRotation, 
                    transform
                );
                
                shell.transform.localScale = cube.transform.localScale;
                    
                Instantiate(
                    SettingsReader.Gs.mapPointPrefab,
                    sliceItemModel.initialLocalPosition,
                    sliceItemModel.initialRotation,
                    shell.transform
                );
            }
            
            CustomGameEvents.Current.MapCreated(transform);
            
        }
    }
}