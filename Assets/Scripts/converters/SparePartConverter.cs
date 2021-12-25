using models.sparepart;
using states.controllers;
using UnityEngine;

namespace converters
{
    public static class SparePartConverter
    {
        public static SparePartStateManager TransformToCarouselItem(Transform tr)
        {
            tr.gameObject.AddComponent<Swipable>();
            return tr.gameObject.AddComponent<SparePartStateManager>();
        }
    }
}