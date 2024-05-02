using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace rx
{
    public class RxEvents : MonoBehaviour
    {
        public Button button;
        private CompositeDisposable compositeDisposable;

        private void Start()
        {
            compositeDisposable = new CompositeDisposable();
            
            var spaceObservable = Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Select(_ => Unit.Default);
            
            var buttonObservable = button
                .OnClickAsObservable()
                .Select(_ => Unit.Default);
            
            spaceObservable.Merge(buttonObservable)
                .ThrottleFirst(TimeSpan.FromSeconds(3))
                .Subscribe(_ => ActAsOnClick())
                .AddTo(compositeDisposable);
            
        }
        
        private void OnDestroy()
        {
            compositeDisposable.Dispose();
        }
        

        private void ActAsOnClick()
        {
            Debug.Log("Text");
        }
    }
}
