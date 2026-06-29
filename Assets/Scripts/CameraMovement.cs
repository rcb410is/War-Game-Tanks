using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.IsPressed())
        {
            Camera.main.transform.position = Camera.main.transform.position;
        }

    }

}
