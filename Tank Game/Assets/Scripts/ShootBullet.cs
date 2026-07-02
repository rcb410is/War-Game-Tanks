using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] SelectionHandler selectionHandler;
    [SerializeField] GameObject player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform tankBarrel;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] float bulletSpeed = 2000;
    [SerializeField] float shotArmTime = 0.5f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    public static Text buttonText;
    public static bool isSelected;
    public static bool isAimReady;
    bool isCurrentTank;
    bool isShotReady = false;
    bool isRotating;
    Vector3 targetDirection;
    Vector3 targetAim;
    Vector3 targetPos;
    RaycastHit hit;

    public void WaitForShot()
    {
        Invoke(nameof(ReadyAim), 0.1f);

    }

    public void ReadyAim()
    {
        isAimReady = true;

    }

    public void AssignText(Text currentButtonText)
    {
        buttonText = currentButtonText;
        buttonText.text = "Click to shoot";
    }

     void Rotation(Vector3 targetDirection)
    {
        targetDirection = targetPos - player.transform.position;
        if (Quaternion.Angle(player.transform.rotation, Quaternion.LookRotation(targetDirection)) > 1f)
        {
            Vector3 newDirection = Vector3.RotateTowards(player.transform.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            player.transform.rotation = Quaternion.LookRotation(newDirection);
            selectionIndicator.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            isRotating = false;
            //Debug.Log("No longer rotating (Method)");
        }

    }

    void Shoot()
    {
        if ((hit.collider.CompareTag("Player") && hit.collider.name != player.name) || hit.collider.CompareTag("Ground"))
        {
            targetAim = targetPos - tankBarrel.position;
            bullet = Instantiate(bullet, tankBarrel.position, tankBarrel.rotation);
            bullet.transform.forward = targetAim.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(targetAim.normalized * bulletSpeed);
            buttonText.text = "SHOOT";
            //Debug.Log("Shot complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"{aimReady}");
        isSelected = selectionHandler.IsSelected();
        isCurrentTank = player == selectionHandler.GetPlayer();
        //Debug.Log("Selected: " + selected);
        if (!isSelected && isAimReady)
        {
            isAimReady = false;
            buttonText.text = "SHOOT";
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && isAimReady && isCurrentTank)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(rayOrigin, out hit);
            //Debug.Log("Mouse position read");

            if ((hit.collider.CompareTag("Player") && hit.collider.name != player.name) || hit.collider.CompareTag("Ground"))
            {
                targetPos = hit.point;
                targetDirection = targetPos - tankBarrel.position;
                isRotating = true;
                //Debug.Log("Ready to rotate");
            }
                      
        }

        if (isRotating && isSelected && isCurrentTank)
        {
            //Debug.Log("Rotating");
            Rotation(targetDirection);
            if (isRotating == false)
            {
                isShotReady = true;
                //Debug.Log("No longer rotating");
            }

        }

        if (isShotReady && isSelected && isCurrentTank)
        {
            Invoke("Shoot", shotArmTime);
            isShotReady = false;
            isAimReady = false;
        }
        if (!isCurrentTank && !isSelected)
        {
            isRotating = false;
            isShotReady = false;
            isAimReady = false;
        }

    }

}
