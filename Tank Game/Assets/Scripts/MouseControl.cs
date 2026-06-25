using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mouse.current.position.ReadValue());
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("Left Click.");
        }
    }
}
