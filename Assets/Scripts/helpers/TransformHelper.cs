using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helpers
{
    public static class TransformHelper
    {

        public static IEnumerator MoveFromTo(Transform what, Vector3 a, Vector3 b, float speed) {
            var step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f) {
                t += step; // Goes from 0 to 1, incrementing by step each time
                what.position = Vector3.Lerp(a, b, t);
                yield return new WaitForFixedUpdate();
            }
            what.position = b;
        }
        
        public static IEnumerator ScaleFromTo(Transform what, Vector3 a, Vector3 b, float speed) {
            while (Vector3.SqrMagnitude(a - b) > 0.001)
            {
                Debug.Log($"a: {a}, b: {b}, res: {Vector3.SqrMagnitude(a - b)}");
                what.localScale = Vector3.Lerp(a, b, Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
            what.localScale = b;
        }      
        
        public static IEnumerator ScaleTo(Transform what, Vector3 finalScale, float speed)
        {
            return ScaleFromTo(what, what.localScale, finalScale, speed);
        }
        
        public static IEnumerator MoveFromToByTime(Transform what, Vector3 a, Vector3 b, float seconds) {
            float t = 0;
            while (t <= 1.0f) {
                t += Time.fixedDeltaTime / seconds; // Goes from 0 to 1, incrementing by step each time
                what.position = Vector3.Lerp(a, b, t);
                yield return new WaitForFixedUpdate();
            }
            what.position = b;
        }
        
        public static IEnumerator MoveToByTime(Transform what, Vector3 finalPos, float seconds) {
            float t = 0;
            while (t <= 1.0f) {
                t += Time.fixedDeltaTime / seconds; // Goes from 0 to 1, incrementing by step each time
                what.position = Vector3.Lerp(what.localPosition, finalPos, t);
                yield return new WaitForFixedUpdate();
            }
            what.position = finalPos;
        }
        
        
        public static IEnumerator RotateTo(Transform what, Quaternion finalRotation, float rotateSpeed)
        {
            while (!what.rotation.Approximately(finalRotation, 0.00001f))
            {
                what.rotation = Quaternion.RotateTowards(
                    what.rotation, 
                    finalRotation, 
                    Time.fixedDeltaTime * rotateSpeed); 
                yield return new WaitForFixedUpdate();
            }
        }
        
        public static IEnumerator RotateByTime(Transform what, Quaternion targetRotation, float duration)
        {
            var passedTime = 0f;
            var startRotation = what.rotation;

            while(passedTime < duration)
            {
                var lerpFactor = passedTime / duration;
                lerpFactor = Mathf.SmoothStep(0, 1, lerpFactor);
                what.rotation = Quaternion.Lerp(startRotation, targetRotation, lerpFactor);
                passedTime += Mathf.Min(duration - passedTime, Time.deltaTime);
                yield return null;
            }
            what.rotation = targetRotation;
        }

        
        public static IEnumerator ChangeScaleByTime(Transform what, Vector3 initScale, Vector3 targetScale, float timeTakes)
        {
            float elapsedTime = 0;
            while (elapsedTime < timeTakes)
            {
                what.localScale = Vector3.Lerp(initScale, targetScale, (elapsedTime / timeTakes));
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        public static IEnumerator ChangeScaleByTime(Transform what, Vector3 targetScale, float timeTakes)
        {
            return ChangeScaleByTime(what, what.localScale, targetScale, timeTakes);
        }

        /*public static Bounds GetHierarchicalBounds(GameObject go)
        {
            var boundsArray = new List<Bounds>();
            var center = Vector3.zero;

            foreach ( var child in go.GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.TryGetComponent(out MeshRenderer itemRenderer)) continue;
                if (itemRenderer == null) continue;
                var bounds = itemRenderer.bounds;
                center += bounds.center;
                boundsArray.Add(bounds);
            }

            center /= go.transform.childCount;
            var totalBound = new Bounds(center, Vector3.zero);
 
            foreach (var t in boundsArray)
            {
                totalBound.Encapsulate(t);
            }

            // find center
            /*var xCenter = (totalBound.min.x + totalBound.max.x) / 2;
            var yCenter = (totalBound.min.y + totalBound.max.y) / 2;
            var zCenter = (totalBound.min.z + totalBound.max.z) / 2;
            
            totalBound.center = new Vector3(xCenter, yCenter, zCenter);#1#
            
            return totalBound;
        }*/

        public static IEnumerator MoveScale(MonoBehaviour context, Transform what, Vector3 where, Vector3 finalScale, float timeTaken)
        {
            var moveFunc = context.StartCoroutine(
                MoveFromToByTime(what, what.position, where, timeTaken));

            var scaleFunc = context.StartCoroutine(
                ChangeScaleByTime(what, what.localScale, finalScale, timeTaken));

            //wait until all of them are over
            yield return moveFunc;
            yield return scaleFunc;
        }

        public static IEnumerator SyncRotationAndPosition(MonoBehaviour ctx, Transform source, Transform target, float timeTaken)
        {
            var moveFunc = ctx.StartCoroutine(
                MoveFromToByTime(target, target.position, source.position, timeTaken));

            var rotateFunc = ctx.StartCoroutine(
                RotateByTime(target, source.rotation, timeTaken));

            //wait until all of them are over
            yield return moveFunc;
            yield return rotateFunc;
        }

        public static IEnumerator WrapOnAction(MonoBehaviour ctx, IEnumerator func, Action resultAction)
        {
            var execFunc = ctx.StartCoroutine(func);
            yield return execFunc;
            
        }
        
        public static IEnumerator ExecuteList(MonoBehaviour ctx, IReadOnlyList<IEnumerator> functions, Action callback)
        {
            var cors = new Coroutine[functions.Count];

            for (var i = 0; i < functions.Count; i++)
            {
                cors[i] = ctx.StartCoroutine(functions[i]);
            }
            
            foreach (var cor in cors)
            {
                yield return cor;
            }
            callback();
        }
        
        
        public static IEnumerator MoveRotateScale(
            MonoBehaviour ctx,
            Transform target,
            Vector3 finalPosition,
            Quaternion finalRotation,
            Vector3 finalScale,
            float timeTaken)
        {
            var moveFunc = ctx.StartCoroutine(
                MoveFromToByTime(target, target.position, finalPosition, timeTaken));
            
            var rotateFunc = ctx.StartCoroutine(
                RotateByTime(target, finalRotation, timeTaken));

            var scaleFunc = ctx.StartCoroutine(
                ChangeScaleByTime(target, target.localScale, finalScale, timeTaken));

            yield return rotateFunc;
            yield return moveFunc;
            yield return scaleFunc;
        }

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

        public static Vector3 ChangeScaleRelative(Vector3 relativeTo, Vector3 targetScale)
        {
            return new Vector3(
                targetScale.x / relativeTo.x,
                targetScale.y / relativeTo.y,
                targetScale.z / relativeTo.z
                );
        }
        
        public static void SetGlobalScale(Transform trn, Vector3 globalScale)
        {
            trn.localScale = Vector3.one;
            var lScale = trn.lossyScale;
            trn.localScale = new Vector3 (globalScale.x / lScale.x,globalScale.y / lScale.y,globalScale.z / lScale.z );
        }
        
        public static float CalculateOffset(Transform trn1, Transform trn2)
        {
            return trn1.localPosition.x - trn2.localPosition.x;
        }
        
    }
}