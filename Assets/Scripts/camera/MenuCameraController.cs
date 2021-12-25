using UnityEngine;

namespace camera
{
    public class MenuCameraController : MonoBehaviour
    {
        private Camera _cam;
        public float distanceMultiplier = 1.5f;
        public Transform counterpart;
        [SerializeField] private float distance;
        [SerializeField] private Vector3[] frustumCorners;

        public void Init()
        {
            _cam = GetComponent<Camera>();
            distance = Vector3.Distance(_cam.transform.position, counterpart.position) / distanceMultiplier;
            frustumCorners = GetFrustumCorners();
        }
        
        public Vector3[] GetFrustumCorners()
        {
            var intArr = new Vector3[4];
            _cam.CalculateFrustumCorners(
                new Rect(0, 0, 1, 1),
                distance,
                Camera.MonoOrStereoscopicEye.Mono,
                intArr
            );

            var res = new Vector3[intArr.Length];
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = _cam.transform.TransformPoint(intArr[i]);
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
            var planes = GeometryUtility.CalculateFrustumPlanes(_cam);
            return GeometryUtility.TestPlanesAABB(planes, target.bounds);
        }
        
    }
    
}
