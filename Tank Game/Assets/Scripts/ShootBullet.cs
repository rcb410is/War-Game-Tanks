using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform tankBarrel;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] GameObject bullet;
    public static Text buttonText;
    [SerializeField] float bulletSpeed;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    Vector3 targetDirection;
    Vector3 targetPos;
    bool aimReady;
    bool shotReady = false;
    bool rotating;
    RaycastHit hit;

    public void WaitForShot()
    {      
        Invoke("Shoot", 0.3f);

    }

    public void Shoot()
    {
        aimReady = true;

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
            rotating = false;
            Debug.Log("No longer rotating (Method)");
        }

    }

    private void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame && aimReady)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(rayOrigin, out hit);

            if ((hit.collider.CompareTag("Player") && hit.collider.name != player.name) || hit.collider.CompareTag("Ground"))
            {
                targetPos = hit.point;
                targetDirection = targetPos - tankBarrel.position;
                rotating = true;
                Debug.Log("Ready to rotate");
            }
                      
        }

        if (rotating)
        {
            Debug.Log("Rotating");
            Rotation(targetDirection);
            if (rotating == false)
            {
                shotReady = true;
                Debug.Log("No longer rotating");
            }
        }

        if (shotReady)
        {
            if ((hit.collider.CompareTag("Player") && hit.collider.name != player.name) || hit.collider.CompareTag("Ground"))
            {
                bullet = Instantiate(bullet, tankBarrel.position, tankBarrel.rotation);
                bullet.transform.forward = targetDirection.normalized;
                bullet.GetComponent<Rigidbody>().AddForce(targetDirection.normalized * bulletSpeed);
                buttonText.text = "SHOOT";
                shotReady = false;
                aimReady = false;
                Debug.Log("Shot complete");
            }

        }

    }

}
