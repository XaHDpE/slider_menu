using camera;
using input.slidermenu.controllers;
using input.slidermenu.providers;
using input.slidermenu.view;
using UnityEngine;

namespace input
{
    public class MenuStarter : MonoBehaviour
    {
        public SliderTopController menuTop;
        public MenuCameraController menuCam;
        public Transform elementsTop;

        public void Init()
        {
            var menuViewManager = new SlideMenuViewManager(menuCam);
            menuTop.Init(menuViewManager, new SliderDataProvider(elementsTop, menuTop));
        }
    }
}
