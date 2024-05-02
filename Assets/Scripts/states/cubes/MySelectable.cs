using System;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace states.cubes
{
    public class MySelectable : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<LeanSelectableByFinger>().OnSelectedFinger.AddListener(MyTest);
        }

        private void MyTest(LeanFinger arg0)
        {
            Debug.Log($"{transform.name} is selected");
        }

        private void OnDestroy()
        {
            GetComponent<LeanSelectableByFinger>().OnSelectedFinger.RemoveAllListeners();
        }
        
        

    }
}
