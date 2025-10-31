using UnityEngine;

public class PingVisibility : MonoBehaviour
{
    Camera cam;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (!cam) cam = Camera.main;
        if (!cam) return;
        transform.forward = cam.transform.forward; // face camera
    }
}
