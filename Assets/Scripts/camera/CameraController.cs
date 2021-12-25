using DG.Tweening;
using helpers;
using input;
using UnityEngine;
using bh = helpers.BoundsHelper;

namespace camera
{
    public class CameraController : MonoBehaviour
    {
        // public variables
        
        // private variables
        private Camera _cam;
        [SerializeField] private GameObject target;
        [SerializeField] [Range(0,2)] private float marginPercentage = 1.0f; 
        
        // events and delegates
        public delegate void CarouselCameraSetDelegate(Camera cam, Vector3 target, float zSize);
        public delegate void CarouselCameraReadyDelegate(Camera cam, Vector3 target);
        
        public static event CarouselCameraSetDelegate CarouselCameraSet;
        public static event CarouselCameraReadyDelegate CarouselCameraReady;
        

        private void Awake()
        {
            _cam = transform.GetComponent<Camera>();
        }

        private void Start()
        {
            // Carousel5.CarouselArranged += AlignWith;
            // SelectedController.SelectedArranged += AlignWith;
            // SelectedController.SelectedArrangedSimple += FocusOn2;
            // CustomGameEvents.Current.OnCarouselTopSet += FocusOn2;
        }

        private void OnDestroy()
        {
            // CustomGameEvents.Current.OnCarouselTopSet -= FocusOn2;
        }

        private void OnDisable()
        {
            // Carousel5.CarouselArranged -= AlignWith;
            // SelectedController.SelectedArranged -= AlignWith;
            // SelectedController.SelectedArrangedSimple -= FocusOn2;
        }
        
        

        private void AlignWith(Bounds bounds, Vector3 lookPoint, Vector3 lookDirection, float yAngleOffset)
        {
            Debug.Log($"[bounds: {bounds}, lookPoint: {lookPoint}, lookDirection: {lookDirection}, yAngleOffset: {yAngleOffset}]");
            const float margin = 0.65f;
            var maxExtent = BoundsHelper.GetMaxDimension(bounds) / 2;
            var minDistance = maxExtent * margin / Mathf.Sin(Mathf.Deg2Rad * _cam.fieldOfView / 2.0f);
            _cam.nearClipPlane = minDistance - maxExtent;
            // Debug.Log($"logs: {minDistance}, {maxExtent}");
            _cam.farClipPlane = 140;
            var transform1 = transform;
            transform1.position = lookPoint + lookPoint.normalized * minDistance + transform1.up * yAngleOffset;
            transform1.rotation = Quaternion.LookRotation(-lookPoint);
            CarouselCameraSet?.Invoke(_cam, lookPoint, bounds.extents.z);
            CarouselCameraReady?.Invoke(_cam, lookPoint);
        }



        private void FocusOn(GameObject focusedObject, float margin)
        {
            var bounds = bh.GetBoundsWithChildren(focusedObject);
            var camTransform = _cam.transform;
            var maxExtent = Mathf.Max(Mathf.Max(bounds.extents.x, bounds.extents.y), bounds.extents.z);
            var minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * _cam.fieldOfView / 2f);
            var focusedObjPos = bounds.center;
            camTransform.position = focusedObjPos - Vector3.forward * minDistance;
            _cam.nearClipPlane = minDistance - maxExtent;
        }

        public void FocusOn2(Transform focusOn)
        {
            var seq = DOTween.Sequence();
            var forwardVector = focusOn.forward;
            var finalRot = Quaternion.LookRotation(-forwardVector);
            var endPos = focusOn.localPosition + forwardVector * 5 + focusOn.up * 1.5f;

            Debug.Log($"endPos: {endPos}");
            
            seq
                .Insert(0, transform.DOMove(endPos, 2))
                .Insert(0, transform.DORotateQuaternion(finalRot, 2));
            
            seq.PlayForward();
        }

        public void RotateAround(float delta)
        {
            transform.RotateAround(
                FindObjectOfType<Carousel>().transform.position, 
                Vector3.up, 
                delta * Time.fixedDeltaTime
                );
        }
        
        
    }
}