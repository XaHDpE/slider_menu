using Lean.Touch;
using scroll.scroll.swipe;
using UnityEngine;

public class SwpMgrDup : MonoBehaviour
{
    [SerializeField] public RectTransform swipeSurface;
    public CubeCtr head;

    private LeanFingerDownCanvas _lfd;
    
    public Vector3 startPos, startScale, prevPos;
    
    public CubeCtr newest;
    private CubeCtr prev;
    void Start()
    {
        _lfd = swipeSurface.GetComponent<LeanFingerDownCanvas>();
        var headTransform = head.transform;
        startPos = headTransform.localPosition;
        startScale = headTransform.localScale;
        newest = head;
        _lfd.OnFinger.AddListener(FingerHandler);
    }
    
    
    

    void FingerHandler(LeanFinger arg0)
    {
        // SwitchHead(head, newest);
        SetHead(head);
    }

    private void SwitchHead(CubeCtr last, CubeCtr newOne)
    {
        UnsetHead(last);
        head = newOne;
        SetHead(newOne);
    }


    private int _counter = 0;
    
    private CubeCtr Spawn(CubeCtr previous)
    {
        var res= Instantiate(head, transform);
        res.GetComponent<MeshRenderer>().material.color = Color.blue;
        res.name = "cube";
        res.head = false;
        res.transform.localPosition = startPos;
        res.prev = previous;
        res.offset = 1;
        return res;
    }

    /*private void Locomotor()
    {
        var arr = GetComponentsInChildren<CubeCtr>();
        foreach (var cirItem in arr)
        {
            var currTransform = cirItem.transform;
            var headPos = head.transform.localPosition;
            
            var offset = headPos - currTransform.localPosition;
            currTransform.localPosition = headPos - offset;
        }
    }*/

    private void SetHead(CubeCtr cb)
    {
        cb.name += "_head";
        cb.head = true;
        cb.GetComponent<MeshRenderer>().material.color = Color.red;
        _lfd.OnFinger.AddListener(cb.GetComponent<LeanMultiUpdate>().AddFinger);
    }
    
    private void UnsetHead(CubeCtr cb)
    {
        cb.name = cb.name.Replace("_head", "");
        cb.GetComponent<MeshRenderer>().material.color = Color.blue;
        _lfd.OnFinger.RemoveListener(cb.GetComponent<LeanMultiUpdate>().AddFinger);
    }    

    // Update is called once per frame
    
    void Update()
    {
        // Locomotor();

        if (Vector3.Distance(newest.transform.localPosition, startPos) >= startScale.magnitude)
        {
            // newest = Spawn(prev);
            prev = newest;
            newest = newest.next;
        }

        if (Vector3.Distance(head.transform.localPosition, prevPos) < 0.001f && !newest.Equals(head))
        {
            SwitchHead(head, newest);
        }

        prevPos = head.transform.localPosition;
        prev = newest;
    }
}
