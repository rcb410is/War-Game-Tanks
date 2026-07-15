using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] SelectionHandler selectionHandler;
    [SerializeField] BulletPoolHandler bulletPoolHandler;
    [SerializeField] MouseControl mouseControl;
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
    public static bool isAnyTankSelected;
    public static bool isAnyTankInShootMode;
    public static bool isAnyTanksAimReady;
    public static bool isAnyTankCurrentlyShooting;
    bool isCurrentTank;
    bool isShotReady;
    bool isRotating;
    Vector3 targetPos;
    Vector3 targetDirection;
    Vector3 targetAim;
    RaycastHit hit;

    public bool IsInShootMode() {  return isAnyTankInShootMode; }

    public bool IsCurrentlyShooting() { return isAnyTankCurrentlyShooting; }

    public void ReadyAim()
    {
        if (!IsCurrentlyShooting())
        {
            isAnyTanksAimReady = !isAnyTanksAimReady;
            isAnyTankInShootMode = !isAnyTankInShootMode;
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
            isAnyTanksAimReady = true;
            isAnyTankCurrentlyShooting = false;
            tankBarrel.GetComponent<AudioSource>().PlayOneShot(clip);
            tank.GetComponent<PlayerMovement>().enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        isAnyTankSelected = selectionHandler.IsSelected();
        isCurrentTank = tank == selectionHandler.GetPlayer();

        if (!isAnyTankSelected && isAnyTanksAimReady)
        {
            isAnyTanksAimReady = false;
            buttonText.text = "SHOOT";
            tank.GetComponent<PlayerMovement>().enabled = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && isAnyTanksAimReady && isCurrentTank && !isRotating && !isShotReady)
        {
            hit = mouseControl.GetHitOnce();
            Debug.Log($"{hit.collider} was hit");

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
        if (isRotating && isAnyTankSelected && isCurrentTank)
        {
            isAnyTankCurrentlyShooting = true;
            Rotation(targetDirection);
            if (isRotating == false)
            {
                isShotReady = true;
            }

        }

        if (isShotReady && isAnyTankSelected && isCurrentTank)
        {
            Invoke(nameof(Shoot), shotArmTime);
            isShotReady = false;
            isAnyTanksAimReady = false;
        }

        if (!isCurrentTank && !isAnyTankSelected)
        {
            isRotating = false;
            isShotReady = false;
            isAnyTanksAimReady = false;
            tank.GetComponent<PlayerMovement>().enabled = true;
        }

    }

}
