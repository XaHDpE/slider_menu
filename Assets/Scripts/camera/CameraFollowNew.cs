using UnityEngine;

namespace camera
{
    public class CameraFollowNew : MonoBehaviour
    {
        public float cameraDistance = 2.0f;
    
        private Camera _cam;
        private float _distance;

        private Bounds _targetBounds;
    
        // Start is called before the first frame update
        private void Start()
        {
            _cam = GetComponent<Camera>();
        }

        public void SetTarget(Transform target)
        {
            _targetBounds = target.GetComponent<Renderer>().bounds;
            var objectSizes = _targetBounds.max - _targetBounds.min;
            var objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            var cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * _cam.fieldOfView); // Visible height 1 meter in front
            _distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            _distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        }
        
        private void FixedUpdate()
        {
            _cam.transform.position = _targetBounds.center - _distance * transform.forward;
        }
    }
}