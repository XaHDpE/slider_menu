using models.sparepart;
using states.controllers;

namespace converters
{
    public static class CubeToCarouselItemConverter
    {
        public static SparePartStateManager CubeToCarouselItem(CubeController tr)
        {
            tr.gameObject.AddComponent<Swipable>();
            return tr.gameObject.AddComponent<SparePartStateManager>();
        }
    }
}