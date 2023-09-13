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
            CustomGameEvents.Current.OnObjectSliced += CreateMap;
        }

        private void OnDestroy()
        {
            CustomGameEvents.Current.OnObjectSliced -= CreateMap;
        }

        private void CreateMap(Transform parentObj)
        {
            targetTop = parentObj;
        
            var cubes = targetTop.GetComponentsInChildren<CubeController>();
            positions = new Vector3[cubes.Length];

            for (var i = 0; i < cubes.Length; i++)
            {
                var cube = cubes[i];
                if (cube.transform.childCount > 0)
                {
                    var sliceItemModel = cube.GetComponentInChildren<SliceItemController>().model;
                    positions[i] = sliceItemModel.initialLocalPosition;
                
                    var parent = Instantiate(
                        SettingsReader.Gs.shellPrefab, 
                        sliceItemModel.initialLocalPosition, 
                        sliceItemModel.initialRotation, 
                        transform
                    );
                
                    parent.transform.localScale = cube.transform.localScale;
                    
                    Instantiate(
                        SettingsReader.Gs.mapPointPrefab,
                        sliceItemModel.initialLocalPosition,
                        sliceItemModel.initialRotation,
                        parent.transform
                    );

                }
            }
        }
    }
}