using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Collider playerCol;
    [SerializeField] SelectionHandler selectionHandler;
    
    [SerializeField] float speed = 50f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    
    
    Vector3 targetPos;
    float posStep;

    bool isCurrentTank;
    bool isSelected;
    bool isMoving;

    //Rotates the tank towards a point
    void Rotate(Vector3 targetDirection)
    {
        targetDirection = targetPos - player.transform.position;
        targetDirection.y = 0f;

        if (Quaternion.Angle(player.transform.rotation, Quaternion.LookRotation(targetDirection)) > 5f && (Mathf.Abs(targetDirection.x) > 2f && Mathf.Abs(targetDirection.z) > 2f))
        {
            Vector3 newDirection = Vector3.RotateTowards(player.transform.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            player.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    //Moves the tank towards a point
    void Move(Vector3 targetDirection)
    {

        targetDirection = targetPos - player.transform.position;
        targetDirection.y = 0f;

        if (Quaternion.Angle(player.transform.rotation, Quaternion.LookRotation(targetDirection)) <= 5f)
        {
            posStep = speed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, posStep);
        }

    }
    
    //Prevents tanks from running into each other
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isMoving = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        isSelected = selectionHandler.IsSelected();
        isCurrentTank = player == selectionHandler.GetPlayer();
        //Debug.Log("Selected: " + selected);
        //Debug.Log("This tank selected: " +  currentTank);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(rayOrigin, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Ground") && isSelected && isCurrentTank)
                {
                    targetPos = hit.point;
                    isMoving = true;
                }
            }
        }

        Vector3 targetDirection = targetPos - player.transform.position;
        float targetRadius = Mathf.Sqrt(Mathf.Pow(targetDirection.x, 2) + Mathf.Pow(targetDirection.z, 2));
        if (isSelected && targetRadius <= 50 && isCurrentTank)
        {
            if (isMoving)
            {
                Rotate(targetDirection);
                Move(targetDirection);
                //Debug.Log("Rotating... " + player.transform.position + " " + targetPos + " " + targetDirection);
            }

        }
        if (Mathf.Abs(targetDirection.x) < 2f && Mathf.Abs(targetDirection.z) < 2f)
        {
            isMoving = false;
        }

    }

}
