using settings;
using UnityEngine;

namespace input.slidermenu.helpers
{
    public static class SliderMenuHelper
    {
        public static Vector3 SetStartPoint(Camera cam, float delta)
        {
            var res = Mathf.Abs(delta).Equals(delta)
                ? cam.ViewportToWorldPoint(SettingsReader.Instance.sliderMenuSettings.westSpawnPoint)
                : cam.ViewportToWorldPoint(SettingsReader.Instance.sliderMenuSettings.eastSpawnPoint);
            
            // Debug.DrawRay(res, Vector3.up * 5f, Color.green, 100);
            
            return res;
        }
        
    }
}