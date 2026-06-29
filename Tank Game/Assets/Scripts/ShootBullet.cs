using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBullet : MonoBehaviour
{
    
    [SerializeField] Transform tankBarrel;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    Vector3 targetDirection;
    bool shotReady;

    public void WaitForShot()
    {
        Invoke("Shoot", 0.3f);
        /*
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Invoke("Shoot", 0.5f);
        }
        */
        

    }

    public void Shoot()
    {
        /*
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            bullet = Instantiate(bullet, tankBarrel.position, tankBarrel.rotation);
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(rayOrigin, out RaycastHit hit);
            targetDirection = hit.point - tankBarrel.position;
            bullet.transform.forward = targetDirection.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(targetDirection.normalized * bulletSpeed);
        }
        */
        shotReady = true;

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && shotReady)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(rayOrigin, out RaycastHit hit);
            targetDirection = hit.point - tankBarrel.position;

            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Ground"))
            {
                bullet = Instantiate(bullet, tankBarrel.position, tankBarrel.rotation);
                bullet.transform.forward = targetDirection.normalized;
                bullet.GetComponent<Rigidbody>().AddForce(targetDirection.normalized * bulletSpeed);
                shotReady = false;
            }
            
        }

    }

}
