using System;
using System.Collections.Generic;
using UnityEngine;

namespace input.Menu
{
    public class View : MonoBehaviour
    {
        public enum PositionAtScreen
        {
            AtScreen,
            BehindLeftEdge,
            BehindRightEdge
        }
        public event Action<View, PositionAtScreen> OnViewBehindTheScreen;
        private GameObject CurrentData { get; set; }
        public int DataIdx { get; private set; } = -1;

        private Vector3 leftBorder;
        private Vector3 rightBorder;
        private Transform dataRoot;
        private PositionAtScreen currentViewPosition;
        private IReadOnlyList<GameObject> dataSource;
    
        public void InitView(
            Vector3 leftBorderPar, 
            Vector3 rightBorderPar, 
            Transform dataRootPar, 
            IReadOnlyList<GameObject> dataSourcePar)
        {
            leftBorder = leftBorderPar;
            rightBorder = rightBorderPar;
            dataRoot = dataRootPar;
            dataSource = dataSourcePar;
        }

        private void Update()
        {
            var curPosition = transform.localPosition;
            var nextPositionAtScreen = PositionAtScreen.AtScreen;
            if (curPosition.x > rightBorder.x)
                nextPositionAtScreen = PositionAtScreen.BehindRightEdge;
            else if (curPosition.x < leftBorder.x)
                nextPositionAtScreen = PositionAtScreen.BehindLeftEdge;

            if (currentViewPosition != nextPositionAtScreen && nextPositionAtScreen != PositionAtScreen.AtScreen)
                OnViewBehindTheScreen?.Invoke(this, nextPositionAtScreen);
            currentViewPosition = nextPositionAtScreen;
        }

        public void SetData(int index)
        {
            ResetData();
            DataIdx = index;
            var data = dataSource[index];
            data.transform.position = Vector3.zero;
            data.transform.rotation = Quaternion.identity;
            data.transform.SetParent(gameObject.transform,false);
            CurrentData = data;
        }

        private void ResetData()
        {
            if (CurrentData == null) return;
            CurrentData.transform.SetParent(dataRoot, false);
            CurrentData = null;
            DataIdx = -1;
        }
    }
}