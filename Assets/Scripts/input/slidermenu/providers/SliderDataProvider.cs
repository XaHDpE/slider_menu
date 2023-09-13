using System.Collections.Generic;
using cubes;
using helpers;
using input.slidermenu.controllers;
using models.cube;
using slicing.controllers;
using UnityEngine;

namespace input.slidermenu.providers
{
    public class SliderDataProvider
    {
        private SliderMenuItemController[] items;

        public SliderDataProvider(Transform slicesTop, Component menuTop)
        {
            var menuItems = new List<SliderMenuItemController>();
            foreach (var cc in slicesTop.GetComponentsInChildren<CubeController>())
            {
                if (cc.GetComponentInChildren<SliceItemController>() != null)
                {
                    var smi = cc.gameObject.AddComponent<SliderMenuItemController>();
                    TransformHelper.ChangeLayersRecursively(smi.transform, "Slider Menu");
                    smi.transform.SetParent(menuTop.transform);
                    smi.MoveSmiToInactive();
                    menuItems.Add(smi);
                }
            }
            
            // Debug.Log($"menuItems: {menuItems.Count}");
            for (var i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].model = new SliderMenuItem()
                {
                    next = menuItems[(menuItems.Count - 1 + i) % menuItems.Count],
                    prev = menuItems[(menuItems.Count + 1 + i) % menuItems.Count]
                };
            }
            items = menuItems.ToArray();
        }

        public SliderMenuItemController[] Items => items;
        
        /*public void SetPrevAsNewCurrent()
        {
            current = current.model.prev;
        }

        public void SetNextAsNewCurrent()
        {
            current = (current == null) ? items[0] : current.model.next;
        }

        public SliderMenuItemController Head => head;
        
        public void SetHead()
        {
            if (head != null) head.MoveSmiToActive();
            head = current;
            head.MoveSmiToHead();
            SwipeMenuEvents.Current.HeadChanged(current);
        }

        public bool CurrentIsHead()
        {
            return current.Equals(head);
        }*/
        
    }
}