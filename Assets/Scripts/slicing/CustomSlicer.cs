using System;
using System.Collections.Generic;
using BzKovSoft.ObjectSlicer;
using cubes;
using events;
using extensions;
using helpers;
using settings;
using slicing.controllers;
using slicing.models;
using UnityEngine;

namespace slicing
{
    public class CustomSlicer : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private int numberOfSlicesPerMinimumSize;
        [SerializeField] private Material internalMaterial;
        
        private int numOfPartsX;
        private int numOfPartsY;
        private int numOfPartsZ;        

        // private Vector3 cubeSizeV;
    
        private void IterateSlicers(Transform parentObj, float numOfParts, Vector3 startPoint, Axis axis,  float dimensionSize)
        {
        
            foreach (var currPiece in parentObj.GetComponentsInChildren<Transform>())
            {
                if (currPiece.transform.Equals(parentObj)) continue;
                var currSlice = currPiece.gameObject;
                for (var k = 1; k < numOfParts; k++)
                {
                    var inPoint = startPoint + axis.Direction * dimensionSize * k / numOfParts;
                    var nPlane = new Plane(axis.Direction, inPoint);
                
                    Cut(currSlice, "it" ,nPlane, out var posGo, out var negGo);
                
                    if (negGo != null && posGo != null)
                    {
                        currSlice = posGo; 
                    }
                    else
                    {
                        currSlice = currPiece.gameObject;
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
        
            numOfPartsX = (int) Math.Ceiling(b.size.x / cutSize);
            numOfPartsY = (int) Math.Ceiling(b.size.y / cutSize);
            numOfPartsZ = (int) Math.Ceiling(b.size.z / cutSize);
        
            /*Debug.Log($"xSize: {b.size.x}, ySize: {b.size.y}, zSize: {b.size.z}. minDimension: {minDimension}." +
                  $"cutSize: {cutSize}, numOfCutsX: {numOfPartsX}, numOfCutsY: {numOfPartsY}, numOfCutsZ: {numOfPartsZ}"
        );*/
        
            IterateSlicers(parentObj, numOfPartsX, b.min, Axis.X, numOfPartsX * cutSize);
            IterateSlicers(parentObj, numOfPartsY, b.min, Axis.Y, numOfPartsY * cutSize);
            IterateSlicers(parentObj, numOfPartsZ, b.min, Axis.Z, numOfPartsZ * cutSize);

            // DrawCubes(b.min, b, minDimension / (numberOfSlicesPerMinimumSize+1));

            DrawCubes(b.min, new Vector3(numOfPartsX, numOfPartsY, numOfPartsZ), cutSize);
        
            BindObjects(parentObj);

            // restore size of the cube (Vector3.one)
            // parentObj.localScale /= cubeSizeV.x;
        
            // Round(filteredCubes);
            CustomGameEvents.Current.ObjectSliced(
                parentObj, 
                new Vector3Int(numOfPartsX, numOfPartsY, numOfPartsZ)
                ); 

        }

        private SliceItem GetSliceInitParams(Transform tr)
        {
            var ccTransform = tr.transform;
            return new SliceItem()
            {
                initialLocalPosition = ccTransform.localPosition,
                initialScale = ccTransform.localScale,
                initialRotation = ccTransform.rotation
            };
        }
        
        private void BindObjects(Transform parent)
        {
            foreach (var si in parent.GetComponentsInChildren<SliceItemController>())
            {
                if (si.name.Equals(parent.name)) continue;
                
                var possibleParents = new Dictionary<Transform, int>();

                foreach (var cube in parent.GetComponentsInChildren<CubeController>())
                {
                    var cubeTransform = cube.transform;
                    var cubeColl = cubeTransform.GetComponent<BoxCollider>();

                    if (BoundsHelper.IsInside(si.GetComponent<MeshFilter>().mesh.bounds.center, cubeColl))
                    {
                        if (possibleParents.ContainsKey(cube.transform))
                        {
                            possibleParents[cubeTransform] = possibleParents[cube.transform] + 1;
                        }
                        else
                        {
                            possibleParents.Add(cube.transform, 1);
                        }

                        si.transform.SetParent(cube.transform);
                        si.model = GetSliceInitParams(cube.transform);
                    }
                }

                if (possibleParents.Count > 1)
                {
                    foreach (var pt in possibleParents)
                    {
                        Debug.Log($"!!!--- for object [{si}], the parent [{pt.Key}] is found ({pt.Value}) vertices");
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

                        PlaceCube(nPos, edgeSize, new Vector3Int(i, j, k));   
                    }
                }
            }
        }
        private void PlaceCube(Vector3 pos, float sizeVal, Vector3Int placeExtended)
        {
            var size = new Vector3(sizeVal, sizeVal, sizeVal);
            var cube = Instantiate(SettingsReader.Gs.shellPrefab, pos, Quaternion.identity);
            cube.transform.SetParent(target.transform.parent);
            cube.transform.localPosition = pos;
            cube.transform.localScale = size;
            
            cube.GetComponent<CubeController>().PlaceExtended = placeExtended;
            
        }
        private void Cut(GameObject targetGo, string newNamePrefix, Plane pln, out GameObject posGo, out GameObject negGo)
        {
            posGo = null;
            negGo = null;

            // print($"targetGo: {targetGo}");
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
            //Debug.Log($"SliceResult: {sliceResult}");

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
                // mf1.gameObject.AddComponent<SparePartController>();
                mf1.gameObject.AddComponent<SliceItemController>();
                posGo = posObj;
            }
        }
    
    }
}