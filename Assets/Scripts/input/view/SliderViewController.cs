using camera;
using input.model;
using settings;
using UnityEditor.Searcher;
using UnityEngine;

namespace input.view
{
    public class SliderViewController : MonoBehaviour
    {
        
        public MenuCameraController cam;
        public Transform sliderContainer;
        private SlideItem[] _items;

        public void SetData(SlideItem[] inData)
        {
            _items = inData;
            Distribute();
        }

        private void Distribute()
        {
            cam.Init();
            var numOfCubes = SettingsReader.Sms.numberOfCubes;
            var cubeDimensionSize = cam.GetStepSize(numOfCubes);
            var startPos = cam.GetFrustumCorners()[0] + Vector3.up * cubeDimensionSize;
            
            var dirV = cam.GetParallelDirVector();
            
            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i].transform;
                var itemSi = _items[i];
                ScaleCube(item, cubeDimensionSize);
                item.SetParent(sliderContainer);
                itemSi.EnableForeignListener();
                item.position = startPos + dirV * (i * cubeDimensionSize * 1.5f);
            }
        }
        
        private static void ScaleCube(Transform target, float targetSize)
        {
            var currentSize = target.GetComponent<MeshRenderer>().bounds.size;
            var curScale = target.localScale;
            var nScale = new Vector3(
                targetSize * curScale.x / currentSize.x,
                targetSize * curScale.y / currentSize.y,
                targetSize * curScale.z / currentSize.z
                );
            target.localScale = nScale;
        }

        private static void RecalculateScale(Transform parent, Transform target)
        {
            var parentScale = parent.localScale;
            var nScale = target.localScale;
            nScale = new Vector3(
                nScale.x / parentScale.x,
                nScale.y / parentScale.y,
                nScale.z / parentScale.z
            );
            target.localScale = nScale;
        }
        
        public static Vector3 GetSpawnPoint(bool isWest, Vector3 initPoint, float offset)
        {
            return initPoint +
                   Vector3.right * (isWest ? offset : -offset) / 2 +
                   Vector3.up * offset / 2 +
                   Vector3.back * offset / 2;
        }
        
    }
}