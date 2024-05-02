using camera;
using helpers;
using input.slidermenu.controllers;
using settings;
using UnityEngine;

namespace input.slidermenu.view
{
    
    public class SlideMenuViewManager
    {

        public readonly MenuCameraController menuCam;
        private Vector3 eastPoint, westPoint, startScale, spawnPoint;

        private readonly ViewPortHelper vph;
        
        public SlideMenuViewManager(MenuCameraController mc)
        {
            menuCam = mc;
            vph = new ViewPortHelper(mc.cam, CalculateSliderMenuCamOffset());

            startScale = CalculateItemCubeSize();
            CalculateSpawnPositions(startScale.x);
            
            SetSpawnPoint(0);
            // Debug.Log($"eastPoint: {eastPoint}, westPoint: {westPoint}, " + $"SpawnPoint: {spawnPoint}, slideDirection: {GetSlideDirection()}");
        }

        public SliderMenuItemController SpawnAtStart(SliderMenuItemController tr)
        {
            var trans = tr.transform;
            trans.localScale = startScale;
            trans.position = spawnPoint;
            return tr;
        }
        
        public void RescaleCanvas(RectTransform canvas)
        {
            var totalHeight = vph.GetVpLeftTop().y - vph.GetVpLeftBottom().y;
            var totalHeightPx = Screen.height;
            var newScale = canvas.localScale;
            var relSize = startScale.y / totalHeight;
            // Debug.Log($"totalH: {totalHeight}, resSize: {relSize}, startScale.y: {startScale.y}");
            newScale.y = relSize;
            canvas.localScale = newScale;
        }
        public float GetItemSize()
        {
            return startScale.magnitude;
        }
        
        private void CalculateSpawnPositions(float objSize)
        {
            eastPoint = vph.GetVpRightBottom() + GetSlideDirection() * objSize + Vector3.up * objSize / 2;
            Debug.DrawRay(eastPoint, Vector3.up, Color.yellow, 100);
            westPoint = vph.GetVpLeftBottom() - GetSlideDirection() * objSize + Vector3.up * objSize / 2;
            Debug.DrawRay(westPoint, Vector3.up, Color.yellow, 100);
        }

        private float CalculateSliderMenuCamOffset()
        {
            var cpPos = menuCam.counterpart.position;
            var mcPos = menuCam.transform.position;
            // var moveVc = (mcPos - cpPos).normalized;
            var dist = Vector3.Distance(mcPos, cpPos) / 2;
            return dist;
        }

        private Vector3 CalculateItemCubeSize()
        {
            var cubeSide = Vector3.Distance(
                               vph.GetVpLeftBottom(), 
                               vph.GetVpLeftTop()) 
                           * SettingsReader.Instance.sliderMenuSettings.relativeVpItemSize;

            var totalLen = Vector3.Distance(vph.GetVpLeftBottom(), vph.GetVpRightBottom());
            var rowSize = totalLen / SettingsReader.Instance.sliderMenuSettings.numberOfCubes;
            var cubeSz = rowSize * (1 - SettingsReader.Instance.sliderMenuSettings.relativeItemInterval);
            // Debug.Log($"row size: {rowSize}, cubeSz: {cubeSz} total: {totalLen}, cubeSide: {cubeSide}");
            return new Vector3(cubeSz, cubeSz , cubeSz);
        }

        public void SetSpawnPoint(float delta)
        {
            var res = Mathf.Abs(delta).Equals(delta) ? westPoint : eastPoint;
            spawnPoint = res;
        }

        public Vector3 GetEastPointPosition()
        {
            return eastPoint;
        }
        
        public Vector3 GetWestPointPosition()
        {
            return westPoint;
        }
        
        public Vector3 GetSpawnPointPosition()
        {
            return spawnPoint;
        }

        public Vector3 GetStartScale()
        {
            return startScale;
        }

        public Vector3 GetSlideDirection()
        {
            return (vph.GetVpRightBottom() - vph.GetVpLeftBottom()).normalized;
        }
        
    }
}