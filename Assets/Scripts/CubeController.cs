using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Vector3 initialLocalPosition;
    public Vector3 initialScale;
    public Quaternion initialRotation;


    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"---- : collision with : {other.gameObject.name}");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"---- : trigger with : {other.gameObject.name}");
    }
    
}
