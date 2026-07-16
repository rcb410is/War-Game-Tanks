using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] int cameraSpeed = 50;
    InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Camera.main.transform.position = Camera.main.transform.position;
        Camera.main.transform.position += new Vector3(moveValue.x * Time.deltaTime * cameraSpeed, 0, moveValue.y * Time.deltaTime * cameraSpeed);

    }

}
