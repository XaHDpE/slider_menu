using System;
using System.Collections.Generic;
using events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ui
{
    public class NavSliderController : MonoBehaviour
    {
        private Slider slider;
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        private void Start()
        {
            CustomGameEvents.Current.OnObjectSliced += AdjustSnap;
            slider = GetComponent<Slider>();
            
            
            
            slider
                .OnValueChangedAsObservable()
                .Subscribe(OnNext)
                .AddTo(disposables);
        }

        private void OnNext(float obj)
        {
            Debug.Log(slider.value);
        }

        private void OnDestroy()
        {
            CustomGameEvents.Current.OnObjectSliced -= AdjustSnap;

            foreach (var disposable in disposables)
                disposable.Dispose();
            disposables.Clear();
            
        }
    
        private void AdjustSnap(Transform parentObj, Vector3Int counts)
        {
            slider.maxValue = counts.z;
        }
    
    }
}
