using UnityEngine;

namespace scroll.scroll.swipe
{
    public class SwpDataProvider
    {
        
        public static CubeCtr[] Fill(Transform parentObj, int itemsCount, int maxVisible)
        {
            var items = new CubeCtr[itemsCount];
            for (var i = 0; i < itemsCount; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // var cube = go.gameObject.AddComponent<CubeCtr>();
                var cube = go.gameObject.AddComponent<CubeCtr>();
                cube.gameObject.SetActive(false);
                cube.name += $"_{i}";
                cube.transform.SetParent(parentObj);
                items[i] = cube;
            }
            
            for (var i = 0; i < itemsCount; i++)
            {
                items[i].next = items[(items.Length - 1 + i) % items.Length];
                items[i].prev = items[(items.Length + 1 + i) % items.Length];
            }
            
            return items;
        }

    }
}