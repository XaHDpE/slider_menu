using UnityEngine;

namespace helpers
{
    public static class TransformHelper
    {

        public static Vector3 ChangeScaleToMax(float maxSize, Transform target)
        {
            var bounds = target.GetComponent<Renderer>().bounds;
            var sz = bounds.size;

            var maxDimension = BoundsHelper.GetMaxDimension(bounds);

            var scale = target.localScale;

            if (bounds.size.x.Equals(maxDimension))
            {
                scale.x = maxSize * scale.x / sz.x;
                scale.z = scale.x;
                scale.y = scale.x;
            }
            else if (bounds.size.y.Equals(maxDimension))
            {
                scale.y = maxSize * scale.y / sz.y;
                scale.z = scale.y;
                scale.x = scale.y;
            }
            else
            {
                scale.z = maxSize * scale.z / sz.z;
                scale.y = scale.z;
                scale.x = scale.z;
            }

            return scale;
        }
        
        public static void ScaleFixed(GameObject target, float newSize) {
            var size = target.GetComponent<Renderer>().bounds.size.y;
            var rescale = target.transform.localScale;
            rescale.x = newSize * rescale.x / size;
            rescale.y = newSize * rescale.y / size;
            rescale.z = newSize * rescale.z / size;
            target.transform.localScale = rescale;
        }

        public static void SetGlobalScale(Transform trn, Vector3 globalScale)
        {
            trn.localScale = Vector3.one;
            var lScale = trn.lossyScale;
            trn.localScale = new Vector3(globalScale.x / lScale.x, globalScale.y / lScale.y, globalScale.z / lScale.z);
        }

        public static float CalculateOffset(Transform trn1, Transform trn2)
        {
            return trn1.localPosition.x - trn2.localPosition.x;
        }

        public static void ChangeLayersRecursively(this Transform trans, string name)
        {
            foreach (var child in trans.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer(name); // add any layer you want. 
            }
        }
    }
}