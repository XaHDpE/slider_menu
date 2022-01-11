using System.Collections.Generic;
using cubes;
using events;
using input.slidermenu.controllers;
using settings;
using UnityEngine;

public class MapController : MonoBehaviour
{

    private Transform targetTop;
    public Vector3[] positions;
    
    public Dictionary<Vector3, Vector3> posRots = new Dictionary<Vector3, Vector3>();

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
            positions[i] = cubes[i].model.initialLocalPosition;
            
            // Debug.Log($"pp_1 : {_targetTop.TransformPoint(cubes[i].initialLocalPosition)}");
            // Debug.Log($"pp_2 : {cubes[i].transform.rotation}");

            // var cubeNew = Instantiate(cubes[i], transform, true);
            // cubeNew.transform.rotation = cubeNew.initialRotation;
            // cubeNew.name = $"map_{i}";

            if (cubes[i].transform.childCount > 0)
            {
                var parent = Instantiate(
                    SettingsReader.Gs.shellPrefab,
                    cubes[i].model.initialLocalPosition,
                    cubes[i].model.initialRotation,
                    transform
                );
                
                parent.transform.localScale = cubes[i].transform.localScale;
                    
                var child = Instantiate(
                    SettingsReader.Gs.mapPointPrefab,
                    cubes[i].model.initialLocalPosition,
                    cubes[i].model.initialRotation,
                    parent.transform
                );

            }
        }
    }
}