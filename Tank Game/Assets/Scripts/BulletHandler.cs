using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
        
    }

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(true);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnBullet(gameObject);
    }

}
