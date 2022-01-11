using cubes;
using input.slidermenu.controllers;
using input.slidermenu.models;
using UnityEngine;

 namespace input.slidermenu.providers
{
    public static class SliderDataProvider
    {
        
        public static SliderMenuItemController[] Fill(Transform parentObj, int itemsCount)
        {
            var items = new SliderMenuItemController[itemsCount];
            for (var i = 0; i < itemsCount; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // var cube = go.gameObject.AddComponent<CubeCtr>();
                // cube.gameObject.SetActive(false);
                go.name += $"_{i}";
                go.transform.SetParent(parentObj);
                items[i] = go.gameObject.AddComponent<SliderMenuItemController>();
                items[i].MoveToInactive();
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