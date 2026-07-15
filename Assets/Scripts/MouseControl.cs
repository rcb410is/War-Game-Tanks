using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{

    bool didRaycastHit;
    RaycastHit hit;

    public RaycastHit GetHit() { return hit; }

    public bool DidRaycastHit() { return didRaycastHit; }

    public RaycastHit GetHitOnce()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
        Physics.Raycast(rayOrigin, out RaycastHit hit);
        return hit;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
        didRaycastHit = Physics.Raycast(rayOrigin, out hit);
    }
}
