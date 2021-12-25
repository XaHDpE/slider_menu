using System.Collections.Generic;
using helpers;
using input.model;
using settings;
using UnityEngine;

namespace input.controller
{
    public static class SliderDataProvider
    {
        public static SlideItem[] FillArray(Transform itemsTop)
        {
            var cubes = itemsTop.GetComponentsInChildren<CubeController>();
            var items = new List<SlideItem>();
            for (var index = 0; index < cubes.Length; index++)
            {
                var cube = cubes[index];
                
                if (cube.transform.childCount == 0) continue;
                
                var si = cube.gameObject.AddComponent<SlideItem>();
                si.Init();
                si.name += $"_{index}";
                CopyHelper.MoveHierarchyToLayer(si.transform, SettingsReader.Sms.menuLayer);
                items.Add(si);
            }
            return items.ToArray();
        }
        
    }
}