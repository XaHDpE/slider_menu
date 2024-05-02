using System.Collections.Generic;
using System.Linq;
using camera;
using cubes;
using input.slidermenu.view;
using UnityEngine;

namespace input.Menu
{
    public class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private float deltaBetweenObjects;
        [SerializeField] private GameObject dataRoot;
        [SerializeField] private GameObject leftBorder;
        [SerializeField] private GameObject rightBorder;
        [SerializeField] private View viewPrefab;
        [SerializeField] private MenuCameraController cameraController;

        private int leftmostViewIdx;
        private int rightmostViewIdx;
        private readonly List<View> viewPool = new List<View>();

        private readonly List<(View, View.PositionAtScreen)> viewBehindTheScreenCallbackList = new List<(View, View.PositionAtScreen)>();
        private List<GameObject> data;
        private int ElemsOnScreenCount => viewPool.Count;

        private SlideMenuViewManager smv;
        // Start is called before the first frame update

        public void Init()
        {
            smv = new SlideMenuViewManager(cameraController);            
            data = dataRoot.transform.GetComponentsInChildren<CubeController>().Select(o => o.gameObject).ToList();
            PlaceSpawnPoints();
            
            CalculateDelta();
            DistributeViews();
            FillViewWithInitialData();
        }

        private void CalculateDelta()
        {
            deltaBetweenObjects = smv.GetItemSize();
            Debug.Log($"deltaBetweenObjects.1: {deltaBetweenObjects}");
        }
        
        private void PlaceSpawnPoints()
        {
            leftBorder.transform.position = smv.GetWestPointPosition();
            rightBorder.transform.position = smv.GetEastPointPosition();
            Debug.Log($"deltaBetweenObjects.2: {Vector3.Distance(leftBorder.transform.position, rightBorder.transform.position)}");
        }

        private void FillViewWithInitialData()
        {
            for (var i = 0; i < ElemsOnScreenCount; i++)
            {
                var curView = viewPool[i];
                curView.SetData(i);
            }
        }

        private void DistributeViews()
        {
            leftmostViewIdx = 0;
            rightmostViewIdx = -1;
            var deltaVector = new Vector3(1, 0, 0) * 0.2f;
            // var deltaVector = smv.GetSlideDirection();
            var curPos = leftBorder.transform.localPosition + deltaVector;
            
            while (curPos.x < rightBorder.transform.localPosition.x)
            {
                var nView = Instantiate(viewPrefab).GetComponent<View>();

                Transform transform1;
                (transform1 = nView.transform).SetParent(transform, false);
                transform1.localPosition = curPos;
                
                nView.gameObject.layer = LayerMask.NameToLayer("Slider Menu");
                
                // Debug.Log($"nview( {curPos} ): {nView.transform.localPosition}, {nView.transform.position}");
                
                nView.InitView(
                    leftBorder.transform.localPosition, 
                    rightBorder.transform.localPosition, 
                    dataRoot.transform, 
                    data);
                
                nView.OnViewBehindTheScreen += OnViewBehindTheScreen;
                viewPool.Add(nView);
                curPos += deltaVector;
                rightmostViewIdx++;
            }
        }
        

        private void OnViewBehindTheScreen(View view, View.PositionAtScreen positionAtScreen)
        {
            viewBehindTheScreenCallbackList.Add((view, positionAtScreen));
        }

        private void LateUpdate()
        {
            if (!viewBehindTheScreenCallbackList.Any()) return;
            
            var objectsBehindRightEdge = viewBehindTheScreenCallbackList.First().Item2 ==
                                         View.PositionAtScreen.BehindRightEdge;
            if(objectsBehindRightEdge)
                viewBehindTheScreenCallbackList.Sort(SortFromLeftToRight);
            else
                viewBehindTheScreenCallbackList.Sort(SortFromRightToLeft);

            foreach (var (view, positionAtScreen) in viewBehindTheScreenCallbackList)
            {
                MoveViewToOtherEdge(view, positionAtScreen);
            }
                
            viewBehindTheScreenCallbackList.Clear();
        }

        private static int SortFromLeftToRight((View, View.PositionAtScreen) a, (View, View.PositionAtScreen) b)
        {
            return (int)(b.Item1.gameObject.transform.localPosition.x - a.Item1.gameObject.transform.localPosition.x);
        }

        private static int SortFromRightToLeft((View, View.PositionAtScreen) a, (View, View.PositionAtScreen) b)
        {
            return SortFromLeftToRight(a, b) * -1;
        }

        private void MoveViewToOtherEdge(View view, View.PositionAtScreen positionAtScreen)
        {
            //if rightmost view behind the screen 
            if (positionAtScreen == View.PositionAtScreen.BehindRightEdge)
            {
                //update data. Set new data index as data index of leftmost view - 1
                view.SetData(LoopIndex(viewPool[leftmostViewIdx].DataIdx - 1, data.Count));
                //move rightmost view to left border
                view.transform.localPosition = viewPool[leftmostViewIdx].transform.localPosition -
                                               new Vector3(deltaBetweenObjects, 0, 0);
                leftmostViewIdx = rightmostViewIdx;
                //current rightmost is previous before last rightmost 
                rightmostViewIdx = LoopIndex(--rightmostViewIdx, ElemsOnScreenCount);
            }
            else if (positionAtScreen == View.PositionAtScreen.BehindLeftEdge) //if leftmost view behind the screen
            {
                //update data. Set new data index as data index of rightmost view - 1
                view.SetData(LoopIndex(viewPool[rightmostViewIdx].DataIdx + 1, data.Count));
                //move leftmost view to right border
                view.transform.localPosition = viewPool[rightmostViewIdx].transform.localPosition +
                                               new Vector3(deltaBetweenObjects, 0, 0);
                rightmostViewIdx = leftmostViewIdx;
                //current leftmost is next after last leftmost 
                leftmostViewIdx = LoopIndex(++leftmostViewIdx, ElemsOnScreenCount);
            }
        }

        private static int LoopIndex(int index, int maxElemCount)
        {
            if (index >= maxElemCount)
                index = 0;
            else if(index < 0)
                index = maxElemCount - 1;

            return index;
        }
        
    }
}