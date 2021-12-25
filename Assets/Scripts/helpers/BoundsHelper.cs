using System.Linq;
using UnityEngine;

namespace helpers
{
    public static class BoundsHelper
    {
        
        
        public static Bounds GetBoundsWithChildren(GameObject gameObject)
        {
            var parentRenderer = gameObject.GetComponent<Renderer>();
            var childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();
 
            var bounds = parentRenderer != null
                ? parentRenderer.bounds
                : childrenRenderers.FirstOrDefault(x => x.enabled).bounds;

            if (childrenRenderers.Length <= 0) return bounds;
            foreach (var renderer in childrenRenderers)
            {
                if (renderer.enabled)
                    bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }


        private static void OnDrawGizmosSelected(Transform target)
        {
            var myBounds = GetBoundsWithChildren(target.gameObject);
            var center = myBounds.center;
            var size = myBounds.size;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube (center, new Vector3 (size.x,size.y,size.z));
            var maxFaceNormal = GetMaxFaceNormal(myBounds);
            // Gizmos.color = Color.black;
            // Gizmos.DrawRay(myBounds.min , maxFaceNormal);
            var end = target.position + target.forward * myBounds.size.x;
            // Debug.DrawLine(transform.position, end, Color.green);
            Debug.Log($"bounds: {myBounds}");
            
        }

        public static Vector3 GetMaxFaceNormal(GameObject go)
        {
            var bounds = GetBoundsWithChildren(go);
            return GetMaxFaceNormal(bounds);
        }
        
        public static Vector3 GetMaxFaceNormal(Bounds bounds)
        {
            var maxDimension = GetMaxDimension(bounds);
            var p1 = bounds.size.z.Equals(maxDimension) ? 
                new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) : 
                new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            var p2 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            var p3 = bounds.min;
            
            var v1 = p3 - p1;
            var v2 = p2 - p1;

            var vCross = Vector3.Cross(v1, v2);
            
            return vCross;
        }

        public static float GetMaxDimension(Bounds bounds)
        {
            return Mathf.Max(Mathf.Max(bounds.size.x, bounds.size.y), bounds.size.z);
        }

        public static float GetMaxDimension(Transform transform)
        {
            return GetMaxDimension(transform.GetComponent<MeshRenderer>().bounds);
        }
        
        public static bool IsInside(Vector3 pPoint, BoxCollider pBox)
        {
            pPoint = pBox.transform.InverseTransformPoint(pPoint) - pBox.center;
            var lHalfX = (pBox.size.x * 0.5f);
            var lHalfY = (pBox.size.y * 0.5f);
            var l_HalfZ = (pBox.size.z * 0.5f);
            return (pPoint.x < lHalfX && pPoint.x > -lHalfX &&
                    pPoint.y < lHalfY && pPoint.y > -lHalfY &&
                    pPoint.z < l_HalfZ && pPoint.z > -l_HalfZ);
        }
        
        public static bool BoundsIsEncapsulated(Bounds encapsulator, Bounds encapsulating)
        {
            return encapsulator.Contains(encapsulating.min) || encapsulator.Contains(encapsulating.max);
        }
        
    }
}