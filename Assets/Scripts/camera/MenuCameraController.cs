using System;
using UnityEngine;

namespace camera
{
    public class MenuCameraController : MonoBehaviour
    {
        public Camera cam;
        public float distanceMultiplier = 1.5f;
        public Transform counterpart;
        [SerializeField] private float distance;
        [SerializeField] private Vector3[] frustumCorners;

        private void Start()
        {
            cam = GetComponent<Camera>(); 
        }

        public void Init()
        {
            distance = Vector3.Distance(cam.transform.position, counterpart.position) / distanceMultiplier;
            frustumCorners = GetFrustumCorners();
        }
        
        public Vector3[] GetFrustumCorners()
        {
            var intArr = new Vector3[4];
            cam.CalculateFrustumCorners(
                new Rect(0, 0, 1, 1),
                distance,
                Camera.MonoOrStereoscopicEye.Mono,
                intArr
            );

            var res = new Vector3[intArr.Length];
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = cam.transform.TransformPoint(intArr[i]);
            }

            return res;
        }

        public Vector3 GetParallelDirVector()
        {
            return (frustumCorners[3] - frustumCorners[0]).normalized;
        }

        public float GetStepSize(int numOfElements)
        {
            return Vector3.Distance(frustumCorners[0], frustumCorners[3]) / numOfElements;
        }
        
        public bool IsTargetVisible(Renderer target)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);
            return GeometryUtility.TestPlanesAABB(planes, target.bounds);
        }
        
    }
    
}
