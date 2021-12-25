using System;
using System.Collections.Generic;
using BzKovSoft.ObjectSlicer;
using events;
using helpers;
using settings;
using UnityEngine;


public class CustomSlicer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private int numberOfSlicesPerMinimumSize;
    [SerializeField] private Material internalMaterial;

    private Vector3 cubeSizeV;
    
    private void IterateSlicers(Transform parentObj, float numOfParts, Vector3 startPoint, Axis axis,  float dimensionSize)
    {
        
        foreach (var currPiece in parentObj.GetComponentsInChildren<Transform>())
        {
            if (currPiece.transform.Equals(parentObj)) continue;
            var currSlice = currPiece.gameObject;
            
            Debug.Log($"Axis: {axis}: currSlice: {currSlice}");
            
            for (var k = 1; k < numOfParts; k++)
            {
                var inPoint = startPoint + axis.Direction * dimensionSize * k / numOfParts;
                var nPlane = new Plane(axis.Direction, inPoint);
                
                Cut(currSlice, "it" ,nPlane, out var posGo, out var negGo);
                
                if (negGo != null && posGo != null)
                {
                    currSlice = posGo; 
                    //SetPosition(currSlice.transform, axis.Direction, k);
                    // Debuggr.DrawPlane(inPoint, axis.Direction, Color.black);
                }
                else
                {
                    // case when plane does not intersect the spare part (result either Pos or Neg)
                    currSlice = currPiece.gameObject;
                    // Debug.Log($"problem in: {currSlice}, iterator: ");
                    // Debuggr.DrawPlane(inPoint, axis.Direction, Color.red);
                    // currSlice.GetComponent<MeshRenderer>().material.color = Color.red;
                }

            }
        }
    }
    
    public void Slice()
    {
        var b = target.GetComponent<BoxCollider>().bounds;
        var parentObj = target.transform.parent;
        var numOfSpareParts = numberOfSlicesPerMinimumSize + 1;
        
        Debug.DrawRay(b.min, target.transform.right, Color.magenta, 100);
        Debug.DrawRay(b.min, target.transform.up, Color.magenta, 100);
        Debug.DrawRay(b.min, target.transform.forward, Color.magenta, 100);

        var minDimension = Math.Min(Math.Min(b.size.x, b.size.y), b.size.z);
        var cutSize = minDimension / numOfSpareParts;
        
        var numOfPartsX = (float) Math.Ceiling(b.size.x / cutSize);
        var numOfPartsY = (float) Math.Ceiling(b.size.y / cutSize);
        var numOfPartsZ = (float) Math.Ceiling(b.size.z / cutSize);
        
        /*Debug.Log($"xSize: {b.size.x}, ySize: {b.size.y}, zSize: {b.size.z}. minDimension: {minDimension}." +
                  $"cutSize: {cutSize}, numOfCutsX: {numOfPartsX}, numOfCutsY: {numOfPartsY}, numOfCutsZ: {numOfPartsZ}"
        );*/
        
        IterateSlicers(parentObj, numOfPartsX, b.min, Axis.X, numOfPartsX * cutSize);
        IterateSlicers(parentObj, numOfPartsY, b.min, Axis.Y, numOfPartsY * cutSize);
        IterateSlicers(parentObj, numOfPartsZ, b.min, Axis.Z, numOfPartsZ * cutSize);

        // DrawCubes(b.min, b, minDimension / (numberOfSlicesPerMinimumSize+1));

        DrawCubes(b.min, new Vector3(numOfPartsX, numOfPartsY, numOfPartsZ), cutSize);
        
        BindObjects(parentObj);

        FillInitialParams(parentObj);

        // restore size of the cube (Vector3.one)
        // parentObj.localScale /= cubeSizeV.x;
        
        // Round(filteredCubes);
        CustomGameEvents.Current.ObjectSliced(parentObj); 

    }

    private static void FillInitialParams(Transform parent)
    {
        foreach (var cc in parent.GetComponentsInChildren<CubeController>())
        {
            var ccTransform = cc.transform;
            cc.initialLocalPosition = ccTransform.localPosition;
            cc.initialScale = ccTransform.localScale;
            cc.initialRotation = ccTransform.rotation;
        }
    }
    
    private void BindObjects(Transform parent)
    {
        foreach (var spPart in parent.GetComponentsInChildren<SparePartController>())
        {
            var possibleParents = new Dictionary<Transform, int>();
            if (spPart.transform.Equals(parent)) continue;


            foreach (var cube in parent.GetComponentsInChildren<CubeController>())
            {
                var cubeColl = cube.GetComponent<BoxCollider>();
                var cubeTransform = cube.transform;

                if (BoundsHelper.IsInside(spPart.GetComponent<MeshFilter>().mesh.bounds.center, cubeColl))
                {
                    if (possibleParents.ContainsKey(cube.transform))
                        possibleParents[cubeTransform] = possibleParents[cube.transform] + 1;
                    else
                        possibleParents.Add(cube.transform, 1);
                    
                    spPart.transform.SetParent(cubeColl.transform);
                    
                }
            }

            if (possibleParents.Count > 1)
            {
                foreach (var pt in possibleParents)
                {
                    Debug.Log($"!!!--- for object [{spPart}], the parent [{pt.Key}] is found ({pt.Value}) vertices");
                }
            }

        }
    }
    private void DrawCubes(Vector3 startPoint, Vector3 counters, float edgeSize)
    {
        for (var i = 0; i < counters.x; i++)
        {
            for (var j = 0; j < counters.y; j++)
            {
                for (var k = 0; k < counters.z; k++)
                {
                    var nPos = new Vector3(
                        startPoint.x + edgeSize / 2 + edgeSize * i,
                        startPoint.y + edgeSize / 2 + edgeSize * j,
                        startPoint.z + edgeSize / 2 + edgeSize * k
                    );

                    PlaceCube(nPos, edgeSize);   
                }
            }
        }
    }
    private void PlaceCube(Vector3 pos, float sizeVal)
    {
        var size = new Vector3(sizeVal, sizeVal, sizeVal);
        var cube = Instantiate(SettingsReader.Gs.shellPrefab, pos, Quaternion.identity);
        cube.transform.SetParent(target.transform.parent);
        cube.transform.localPosition = pos;
        cube.transform.localScale = size;
        cubeSizeV = size;
    }
    private void Cut(GameObject targetGo, string newNamePrefix, Plane pln, out GameObject posGo, out GameObject negGo)
    {
        posGo = null;
        negGo = null;

        print($"targetGo: {targetGo}");
        var meshFilter = targetGo.GetComponent<MeshFilter>();
        var meshRenderer = targetGo.GetComponent<MeshRenderer>();
        var mesh = meshFilter.mesh;
        var materials = meshRenderer.sharedMaterials;
        var sectionMaterial = internalMaterial;
        var adapter = new BzManualMeshAdapter(mesh.vertices);

        // slice mesh
        var meshDissector =
            new BzMeshDataDissector(mesh, pln, materials, adapter, BzSliceConfiguration.GetDefault())
            {
                DefaultSliceMaterial = sectionMaterial
            };
        var sliceResult = meshDissector.Slice();

        // apply result back to our object
        Debug.Log($"SliceResult: {sliceResult}");

        if (sliceResult == SliceResult.Sliced)
        {
            // negative
            var result = meshDissector.SliceResultNeg;
            meshFilter.name = meshFilter.name + "_" +  newNamePrefix + "_neg";
            meshFilter.mesh = result.GenerateMesh();
            meshRenderer.materials = result.Materials;
            // meshFilter.sharedMesh = mesh;
            meshFilter.mesh.name = "test1";
            negGo = targetGo;

            // positive
            var res1 = meshDissector.SliceResultPos;
            var posObj = new GameObject();
            var mf1 = posObj.AddComponent<MeshFilter>();
            var mr1 = posObj.AddComponent<MeshRenderer>();
            mf1.name = mf1.name + "_" +  newNamePrefix + "_pos";
            mf1.mesh = res1.GenerateMesh();
            mf1.mesh.name = "mesh1";
            mr1.materials = res1.Materials;
            //mf1.sharedMesh = mesh;
            mf1.transform.localScale = negGo.transform.localScale;
            mf1.transform.SetParent(negGo.transform.parent);
            mf1.gameObject.AddComponent<SparePartController>();
            posGo = posObj;
        }
    }
    
}