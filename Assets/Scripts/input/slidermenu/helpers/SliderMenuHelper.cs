using settings;
using UnityEngine;

namespace input.slidermenu.helpers
{
    public static class SliderMenuHelper
    {
        public static Vector3 SetStartPoint(Camera cam, float delta)
        {
            return Mathf.Abs(delta).Equals(delta)
                ? cam.ViewportToWorldPoint(SettingsReader.Instance.sliderMenuSettings.westSpawnPoint)
                : cam.ViewportToWorldPoint(SettingsReader.Instance.sliderMenuSettings.eastSpawnPoint);
        }
        
    }
}