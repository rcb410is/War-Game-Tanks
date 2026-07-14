using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] SelectionHandler selectionHandler;
    [SerializeField] BulletPoolHandler bulletPoolHandler;
    [SerializeField] GameObject tank;
    [SerializeField] GameObject tankBarrel;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] AudioClip clip;
    [SerializeField] float bulletSpeed = 8000;
    [SerializeField] float shotArmTime = 0.5f;
    [SerializeField] float rotMaxRad = 3.0f;
    [SerializeField] float rotMaxMag = 0.1f;
    [SerializeField] bool canShootTeammates;
    public GameObject bullet;
    public static Text buttonText;
    public static bool isSelected;
    public static bool isInShootMode;
    public static bool isAimReady;
    public static bool isCurrentlyShooting;
    bool isCurrentTank;
    bool isShotReady;
    bool isRotating;
    Vector3 targetPos;
    Vector3 targetDirection;
    Vector3 targetAim;
    RaycastHit hit;

    public bool IsInShootMode() {  return isInShootMode; }

    public bool IsCurrentlyShooting() { return isCurrentlyShooting; }

    public void ReadyAim()
    {
        if (!IsCurrentlyShooting())
        {
            isAimReady = !isAimReady;
            isInShootMode = !isInShootMode;
        }
    }

    public void AssignText(Text currentButtonText)
    {
        buttonText = currentButtonText;
        if (buttonText.text == "SHOOT")
        {
            buttonText.text = "Click to shoot";
        }
        else
        {
            buttonText.text = "SHOOT";
        }
    }

    void Rotation(Vector3 targetDirection)
    {
        if (hit.collider.CompareTag("Ground"))
        {
            targetDirection = targetPos - tank.transform.position;
        }
        else if (hit.collider.CompareTag("Player") && hit.collider.name != tank.name)
        {
            targetDirection = hit.collider.transform.position - tank.transform.position;
        }
        if (Quaternion.Angle(tank.transform.rotation, Quaternion.LookRotation(targetDirection)) > 1f)
        {
            Vector3 newDirection = Vector3.RotateTowards(tank.transform.forward, targetDirection, rotMaxRad * Time.deltaTime, rotMaxMag);
            tank.transform.rotation = Quaternion.LookRotation(newDirection);
            selectionIndicator.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            isRotating = false;
        }

    }

    void Shoot()
    {
        Debug.Log($"Hit: {hit.collider.name}, Current tank: {tank.name}");
        if (((hit.collider.CompareTag("Player") || hit.collider.CompareTag("PlayerDeadZone")) && ((hit.collider.name != tank.name) || canShootTeammates)) || hit.collider.CompareTag("Ground"))
        {
            targetAim = targetPos - tankBarrel.transform.position;
            bullet = bulletPoolHandler.GetComponent<BulletPoolHandler>().GetBullet();
            bullet.transform.SetPositionAndRotation(tankBarrel.transform.position, tankBarrel.transform.rotation);
            bullet.transform.forward = targetAim.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(targetAim.normalized * bulletSpeed);
            var particle = tank.GetComponent<ParticleSystem>();
            particle.Play();
            isAimReady = true;
            isCurrentlyShooting = false;
            tankBarrel.GetComponent<AudioSource>().PlayOneShot(clip);
            tank.GetComponent<PlayerMovement>().enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        isSelected = selectionHandler.IsSelected();
        isCurrentTank = tank == selectionHandler.GetPlayer();

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

            if ((hit.collider.CompareTag("Player") && ((hit.collider.name != tank.name) || canShootTeammates)) || hit.collider.CompareTag("Ground"))
            {
                targetPos = hit.point;
                targetDirection = targetPos - tankBarrel.transform.position;
                isRotating = true;
                tank.GetComponent<PlayerMovement>().enabled = false;
            }
                      
        }

    }

    private void FixedUpdate()
    {
        if (isRotating && isSelected && isCurrentTank)
        {
            isCurrentlyShooting = true;
            Rotation(targetDirection);
            if (isRotating == false)
            {
                isShotReady = true;
            }

        }

        if (isShotReady && isSelected && isCurrentTank)
        {
            Invoke(nameof(Shoot), shotArmTime);
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
