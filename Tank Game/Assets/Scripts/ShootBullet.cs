using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] SelectionHandler selectionHandler;
    [SerializeField] GameObject tank;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform tankBarrel;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] float bulletSpeed = 6000;
    [SerializeField] float shotArmTime = 0.5f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    public static Text buttonText;
    public static bool isSelected;
    public static bool isAimReady;
    bool isCurrentTank;
    bool isShotReady;
    bool isRotating;
    Vector3 targetPos;
    Vector3 targetDirection;
    Vector3 targetAim;
    RaycastHit hit;

    public bool IsAimReady() {  return isAimReady; }

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
        targetDirection = targetPos - tank.transform.position;
        if (Quaternion.Angle(tank.transform.rotation, Quaternion.LookRotation(targetDirection)) > 1f)
        {
            Vector3 newDirection = Vector3.RotateTowards(tank.transform.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            tank.transform.rotation = Quaternion.LookRotation(newDirection);
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
        if ((hit.collider.CompareTag("Player") && hit.collider.name != tank.name) || hit.collider.CompareTag("Ground"))
        {
            targetAim = targetPos - tankBarrel.position;
            bullet = Instantiate(bullet, tankBarrel.position, tankBarrel.rotation);
            bullet.transform.forward = targetAim.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(targetAim.normalized * bulletSpeed);
            var particle = tank.GetComponent<ParticleSystem>();
            particle.Play();
            buttonText.text = "SHOOT";
            tank.GetComponent<PlayerMovement>().enabled = true;
            //Debug.Log("Shot complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"{aimReady}");
        isSelected = selectionHandler.IsSelected();
        isCurrentTank = tank == selectionHandler.GetPlayer();
        //Debug.Log("Selected: " + selected);
        if (!isSelected && isAimReady)
        {
            isAimReady = false;
            buttonText.text = "SHOOT";
            tank.GetComponent<PlayerMovement>().enabled = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && isAimReady && isCurrentTank && !isRotating && !isShotReady)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(rayOrigin, out hit);
            //Debug.Log("Mouse position read");

            if ((hit.collider.CompareTag("Player") && hit.collider.name != tank.name) || hit.collider.CompareTag("Ground"))
            {
                targetPos = hit.point;
                targetDirection = targetPos - tankBarrel.position;
                isRotating = true;
                tank.GetComponent<PlayerMovement>().enabled = false;
                //Debug.Log("Ready to rotate");
            }
                      
        }

    }

    private void FixedUpdate()
    {
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
            tank.GetComponent<PlayerMovement>().enabled = true;
        }
    }

}
