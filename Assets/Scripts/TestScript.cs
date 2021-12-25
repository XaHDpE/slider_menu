using System.Collections.Generic;
using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Samples;
using UnityEngine;

public class TestScript : MonoBehaviour {

    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 numOfPlanes;
    private List<Plane> planes = new List<Plane>();

    private void DrawPlanes1()
    {
        foreach (var plane in planes)
        {
            foreach (var sliceable in target.transform.parent.gameObject.GetComponentsInChildren<IBzSliceableNoRepeat>())
            {
                var sliceId = SliceIdProvider.GetNewSliceId();
                sliceable.Slice(plane, sliceId,delegate(BzSliceTryResult i)
                {
                    print(i.outObjectNeg);
                    print(i.outObjectPos);
                });
            }
        }
    }
    
    private void DrawPlanes()
    {
        var mainObj = target.GetComponent<IBzSliceableNoRepeat>();
        Process(mainObj);
    }

    private int counter = 0;
    private int sliceId = 0;
    
    private void Process(IBzSliceableNoRepeat sliceable)
    {
        counter++;
        foreach (var plane in planes)
        {
            sliceId = SliceIdProvider.GetNewSliceId();
            sliceable.Slice(plane, sliceId,delegate(BzSliceTryResult i)
            {
                if (!i.sliced) return;
                Process(i.outObjectPos.GetComponent<IBzSliceableNoRepeat>());
            });    
        }
        print($"{sliceable} is sliced, counter: {counter}, sliceId: {sliceId}");
    }
    

    /*private void OnDrawGizmos()
    {
        var b = target.GetComponent<BoxCollider>().bounds;
        var startPoint = b.min;
        for (var k = 1; k < ( numOfPlanes.z + 1 ); k++)
        {
            var nPos = new Vector3( startPoint.x, startPoint.y, startPoint.z + b.size.z * k / (numOfPlanes.z + 1));
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(nPos, Vector3.forward);
            
        }
    }*/


    public void CalculatePlanes()
    {
        var b = target.GetComponent<BoxCollider>().bounds;
        var currSlice = target;
        var startPoint = b.min;
        
        for (var k = 1; k < ( numOfPlanes.x + 1 ); k++)
        {
            
            var nPos = new Vector3( startPoint.x + b.size.x * k / (numOfPlanes.x + 1), startPoint.y, startPoint.z);
            var nPlane = new Plane(Vector3.left, nPos);
            planes.Add(nPlane);

            var g1 = currSlice.GetComponent<MeshFilter>();
            
            BreakInto2(
                currSlice, 
                nPlane, 
                out var nextSlice, 
                out var skippedSlice);
            currSlice = nextSlice;

        }
        
    }

    private void BreakInto2(GameObject targetGo, Plane pln, out GameObject posGo, out GameObject negGo)
    {
        {
            var meshFilter = targetGo.GetComponent<MeshFilter>();
            var meshRenderer = targetGo.GetComponent<MeshRenderer>();
            var mesh = meshFilter.mesh;
            var materials = meshRenderer.sharedMaterials;
            var sectionMaterial = new Material(Shader.Find("Diffuse"));
            var adapter = new BzManualMeshAdapter(mesh.vertices);

            // slice mesh
            var meshDissector =
                new BzMeshDataDissector(mesh, pln, materials, adapter, BzSliceConfiguration.GetDefault())
                {
                    DefaultSliceMaterial = sectionMaterial
                };
            var sliceResult = meshDissector.Slice();

            // apply result back to our object
            if (sliceResult == SliceResult.Sliced)
            {
                // negative
                var result = meshDissector.SliceResultNeg;
                meshFilter.mesh = result.GenerateMesh();
                meshRenderer.materials = result.Materials;

                negGo = targetGo;
                
                // positive
                var res1 = meshDissector.SliceResultPos;
                var posObj = new GameObject();
                var mf1 = posObj.AddComponent<MeshFilter>();
                var mr1 = posObj.AddComponent<MeshRenderer>();
                mf1.mesh = res1.GenerateMesh();
                mr1.materials = res1.Materials;

                posGo = posObj;

            }
            else
            {
                posGo = null;
                negGo = null;
            }
            
        }
    }
    
    
    private void ProcessRec(IBzSliceableNoRepeat sliceable, IReadOnlyList<Plane> ps, int iteration)
    {
        sliceId = SliceIdProvider.GetNewSliceId();
        var plane = ps[iteration];
        sliceable.Slice(plane, sliceId,delegate(BzSliceTryResult i)
        {
            if (!i.sliced) return;
            ProcessRec(i.outObjectPos.GetComponent<IBzSliceableNoRepeat>(), ps, ++iteration);
        });
    }
    
    public void CalculatePlanes1()
    {
        // var b = CalcColliders();
        var b = target.GetComponent<BoxCollider>().bounds;
        var startPoint = b.min;
        
        for (var k = 1; k < ( numOfPlanes.x + 1 ); k++)
        {
            var nPos = new Vector3( startPoint.x + b.size.x * k / (numOfPlanes.x + 1), startPoint.y, startPoint.z);
            var nPlane = new Plane(Vector3.left, nPos);
            planes.Add(nPlane);
        }
        
        for (var k = 1; k < ( numOfPlanes.y + 1 ); k++)
        {
            var nPos = new Vector3( startPoint.x, startPoint.y + b.size.y * k / (numOfPlanes.y + 1), startPoint.z);
            var nPlane = new Plane(Vector3.up, nPos);
            planes.Add(nPlane);
        }
        
        for (var k = 1; k < ( numOfPlanes.z + 1 ); k++)
        {
            var nPos = new Vector3( startPoint.x, startPoint.y, startPoint.z + b.size.z * k / (numOfPlanes.z + 1));
            var nPlane = new Plane(Vector3.forward, nPos);
            planes.Add(nPlane);
        }
        
        DrawPlanes();
    }
    private Bounds CalcColliders()
    {
        var goChild = new GameObject();
        goChild.transform.SetParent(target.transform);

        var bcc = goChild.AddComponent<BoxCollider>();
        
        var bounds = new Bounds(target.transform.position, Vector3.zero);
        bounds.Encapsulate(target.GetComponent<MeshRenderer>().bounds);

        bcc.size = bounds.size;
        bcc.center = bounds.center;
        
        return bounds;
    }
    
    public void CalculatePlanes12old()
    {

        var b = CalcColliders();
        var startPoint = CalcColliders().min;
        
        for (var i = 1; i < ( numOfPlanes.x + 1 ); i++)
        {
            for (var j = 1; j < ( numOfPlanes.y + 1 ); j++)
            {
                for (var k = 1; k < ( numOfPlanes.z + 1 ); k++)
                {

                    var nPos = new Vector3(
                        startPoint.x + b.size.x * i / (numOfPlanes.x + 1), 
                        startPoint.y + b.size.y * j / (numOfPlanes.y + 1), 
                        startPoint.z + b.size.z * k / (numOfPlanes.z + 1)
                    );

                    var rotations = new [] { Vector3.up, Vector3.forward, Vector3.left };
                    foreach (var normal in rotations)
                    {
                        var newPlane = new Plane(normal, nPos);
                        
                        if (!planes.Contains(newPlane)) planes.Add(newPlane);
                    }
                    
                }
            }            
        }

        DrawPlanes();

    }

}
