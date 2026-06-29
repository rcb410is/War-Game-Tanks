using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Transform point;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] Transform radiusIndicator;
    [SerializeField] Collider playerCol;
    [SerializeField] float speed = 50f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    [SerializeField] float yOffset = 0;
    Vector3 targetPos;
    float posStep;
    bool selected;
    bool moving;

    void Start()
    {
        point = Instantiate(point, new Vector3 (0,-40,0), Quaternion.identity);
        selectionIndicator = Instantiate(selectionIndicator, new Vector3 (0, -45, 0), Quaternion.identity);
        radiusIndicator = Instantiate(radiusIndicator, new Vector3 (0, -50, 0), Quaternion.identity);
    }

    //Rotates the object towards a point
    void Rotation(Vector3 targetDirection)
    {
        targetDirection = targetPos - player.position;
        targetDirection.y = 0f; //targetDirection.y;

        if (Quaternion.Angle(player.rotation, Quaternion.LookRotation(targetDirection)) > 5f && (Mathf.Abs(targetDirection.x) > 2f && Mathf.Abs(targetDirection.z) > 2f))
        {
            Vector3 newDirection = Vector3.RotateTowards(player.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            player.rotation = Quaternion.LookRotation(newDirection);
            selectionIndicator.rotation = Quaternion.LookRotation(newDirection);
        }

    }

    //Moves the object towards a point
    void Move(Vector3 targetDirection)
    {

        targetDirection = targetPos - player.position;
        targetDirection.y = 0f; //player.position.y;

        if (Quaternion.Angle(player.rotation, Quaternion.LookRotation(targetDirection)) <= 5f)
        {
            posStep = speed * Time.deltaTime;
            player.position = Vector3.MoveTowards(player.position, targetPos, posStep);
            selectionIndicator.position = Vector3.MoveTowards(selectionIndicator.position, targetPos, posStep);
            radiusIndicator.position = Vector3.MoveTowards(selectionIndicator.position, targetPos, posStep);
        }

    }

    //Prevents from running into each other
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            moving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            //LayerMask layerMask = LayerMask.GetMask("UI");

            if (Physics.Raycast(rayOrigin, out RaycastHit hit/*, 500.0f, layerMask*/))
            {
                if (hit.collider.Equals(playerCol) && !selected) {
                    selected = true;
                    //Debug.Log("Selected " + player);
                    selectionIndicator.position = player.position;
                    radiusIndicator.position = player.position;
                }
                else if (hit.collider.Equals(playerCol) && selected)
                {
                    selected = false;
                    //Debug.Log("Deselected " + player);
                    selectionIndicator.position = new Vector3 (0, -20, 0);
                    radiusIndicator.position = new Vector3(0, -30, 0);
                    point.position = new Vector3 (0, -10, 0);
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    selected = false;
                    //Debug.Log("Selected other tank");
                    selectionIndicator.position = new Vector3 (0, -20, 0);
                    radiusIndicator.position = new Vector3(0, -30, 0);
                    point.position = new Vector3(0, -10, 0);
                }
                else
                {
                    //Debug.Log("Did Not Select");
                }

                if (hit.collider.CompareTag("Ground") && selected)
                {
                    targetPos = hit.point;
                    point.position = targetPos;
                    moving = true;
                }

            }

        }

        Vector3 targetDirection = targetPos - player.position;
        float targetRadius = Mathf.Sqrt(Mathf.Pow(targetDirection.x, 2) + Mathf.Pow(targetDirection.z, 2));
        if (selected && targetRadius <= 50)
        {
            if (moving)
            {
                Rotation(targetDirection);
                Move(targetDirection);
                Debug.Log("Rotating... " + player.transform.position + " " + targetPos + " " + targetDirection);
            }

        }
        if (Mathf.Abs(targetDirection.x) < 2f && Mathf.Abs(targetDirection.z) < 2f)
        {
            moving = false;
        }

    }

}
