using System;
using System.Collections.Generic;
using states.controllers;
using ui;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace cubes
{

    [RequireComponent(typeof(CubeStateManager))]
    [RequireComponent(typeof(BoxCollider))]
    public class CubeController : MonoBehaviour
    {
        private CubeStateManager stateManager;
        private NavSliderController navSliderController;
        private Vector3Int placeExtended;
        private Highlight highlight;
        
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        private void Awake()
        {
            if (TryGetComponent(out CubeStateManager stMgr))
            {
                stateManager = stMgr;
                // Debug.Log($"CubeStateManager is assigned to the {transform}");
            }
            else
            {
                Debug.LogError($"Something wrong with CubeController for {transform}");
            }

        }

        private void Start()
        {
            navSliderController = (NavSliderController)FindFirstObjectByType(typeof(NavSliderController));
            if (navSliderController && navSliderController.TryGetComponent(out Slider navSlider))
            {
                navSlider
                    .OnValueChangedAsObservable()
                    .Subscribe(ProcessNavSliderChange)
                    .AddTo(disposables);
            }
        }

        private bool ComparePosition(float pos)
        {
            var res = Math.Abs(placeExtended.z - pos) < double.Epsilon;
            // Debug.Log($"pos: {pos}, vector: {placeExtended}, {res}");
            return res;
        }

        private void ProcessNavSliderChange(float cur)
        {
            Debug.Log($"I should be marked as highlighted : {placeExtended}");
            var compareRes = Math.Abs(placeExtended.z - cur) < double.Epsilon;

            if (compareRes)
                highlight.ToggleHighlight(true);
            else if (highlight.state)
                highlight.ToggleHighlight(false);

        }


        public Vector3Int PlaceExtended
        {
            get => placeExtended;
            set => placeExtended = value;
        }

        private void OnDestroy()
        {
            foreach (var disposable in disposables)
                disposable.Dispose();
            disposables.Clear();
        }


        public bool InFront()
        {
            return transform.GetChild(0).GetComponent<Renderer>().sharedMaterials.Length > 1;
        }
        
    }
}