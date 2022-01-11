using input.slidermenu.controllers;
using input.slidermenu.providers;
using UnityEngine;

namespace input
{
    public class MenuStarter : MonoBehaviour
    {
        public Transform menuTop;

        private void Start()
        {
            var ar = SliderDataProvider.Fill(menuTop, 90);
            menuTop.GetComponent<SliderTopController>().Init(ar, 0);
        }
    
    }
}
