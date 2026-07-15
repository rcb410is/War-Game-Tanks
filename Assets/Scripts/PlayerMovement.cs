using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] GameObject tank;
    [SerializeField] SelectionHandler selectionHandler;
    [SerializeField] MouseControl mouseControl;

    [SerializeField] float speed = 50f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    public TanksTypes TankType;

    Vector3 targetPos;
    float posStep;

    bool isCurrentTank;
    bool isSelected;
    public bool isMoving;

    //Rotates the tank towards a point
    void Rotate(Vector3 targetDirection)
    {
        targetDirection = targetPos - tank.transform.position;
        targetDirection.y = 0f;

        if (Quaternion.Angle(tank.transform.rotation, Quaternion.LookRotation(targetDirection)) > 1f && (Mathf.Abs(targetDirection.x) > 2f || Mathf.Abs(targetDirection.z) > 2f))
        {
            Vector3 newDirection = Vector3.RotateTowards(tank.transform.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            tank.transform.rotation = Quaternion.LookRotation(newDirection);
        }

    }

    //Moves the tank towards a point
    void Move(Vector3 targetDirection)
    {
        targetDirection = targetPos - tank.transform.position;
        targetDirection.y = 0f;

        if (Quaternion.Angle(tank.transform.rotation, Quaternion.LookRotation(targetDirection)) <= 5f)
        {
            posStep = speed * Time.deltaTime;
            tank.transform.position = Vector3.MoveTowards(tank.transform.position, targetPos, posStep);
        }

    }

    //Prevents tanks from running into each other
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player1") || collision.collider.CompareTag("Player2"))
        {
            isMoving = false;
            tank.GetComponent<AudioSource>().Stop();
        }

    }
    

    // Update is called once per frame
    void Update()
    {
        isSelected = selectionHandler.IsSelected();
        isCurrentTank = tank == selectionHandler.GetPlayer();
        //Debug.Log("Selected: " + isSelected);
        //Debug.Log("This tank selected: " +  isCurrentTank);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit = mouseControl.GetHitOnce();

            if (!tank.GetComponent<ShootBullet>().IsInShootMode())
            {
                if ((hit.collider.CompareTag("Ground") || hit.collider.CompareTag("PlayerDeadZone")) && isSelected && isCurrentTank)
                {
                    targetPos = hit.point;
                    isMoving = true;
                    Vector3 targetDirection = targetPos - tank.transform.position;

                    float targetRadius = Mathf.Sqrt(Mathf.Pow(targetDirection.x, 2) + Mathf.Pow(targetDirection.z, 2));
                    if (targetRadius <= 50)
                    {
                        MultiplayerHandler.UsedAction();
                    }         
                }
            }
        }

    }

    private void FixedUpdate()
    {
        Vector3 targetDirection = targetPos - tank.transform.position;
        float targetRadius = Mathf.Sqrt(Mathf.Pow(targetDirection.x, 2) + Mathf.Pow(targetDirection.z, 2));
        if (targetRadius <= 50)
        {
            if (isMoving)
            {
                Rotate(targetDirection);
                Move(targetDirection);
                //Debug.Log("Rotating... " + player.transform.position + " " + targetPos + " " + targetDirection);
            }
            if (isMoving && !tank.GetComponent<AudioSource>().isPlaying)
            {
                tank.GetComponent<AudioSource>().Play();
            }

        }
        if (Mathf.Abs(targetDirection.x) < 2f && Mathf.Abs(targetDirection.z) < 2f)
        {
            tank.GetComponent<AudioSource>().Stop();
            isMoving = false;
        }

    }

}
