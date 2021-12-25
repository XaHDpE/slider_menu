using events;
using input.view;
using UnityEngine;

namespace input.controller
{
    [RequireComponent(typeof(SliderViewController))]
    public class SliderController : MonoBehaviour
    {
        private SliderViewController _svc;
        private void Start()
        {
            CustomGameEvents.Current.OnObjectSliced += ArrangeSlider;
            _svc = GetComponent<SliderViewController>();
        }

        private void ArrangeSlider(Transform top)
        {
            var items = SliderDataProvider.FillArray(top);
            _svc.SetData(items);
        }
        
    }
}