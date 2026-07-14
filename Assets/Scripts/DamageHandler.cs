using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{

    [SerializeField] GameObject spawnInstance;
    [SerializeField] GameObject tank;
    [SerializeField] int tankHealth = 2;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            tankHealth--;
            Debug.Log($"{tank.name} has {tankHealth}HP");
        }
        if (tankHealth <= 0)
        {
            //Debug.Log($"{tank.name} was destroyed");
            tankHealth = 2;
            tank.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            spawnInstance.GetComponentInChildren<SpawnInstance>().ReturnTank(tank);
            //tank.GetComponent<Rigidbody>().AddForce(0, 20000, 0);
        }

    }

}
