using UnityEngine;

public class DamageHandler : MonoBehaviour
{

    [SerializeField] GameObject tank;
    [SerializeField] int tankHealth = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            tankHealth--;
            Debug.Log($"{tank.name} has {tankHealth}HP");
        }
        if (tankHealth <= 0)
        {
            Debug.Log($"{tank.name} was destroyed");
            tank.GetComponent<Rigidbody>().AddForce(0, 20000, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (tankHealth <= 0)
        {
            Debug.Log($"{tank.name} was destroyed");
            tank.GetComponent<Rigidbody>().AddForce(0, 4000 * Time.deltaTime, 0);
        }
        */
    }

}
