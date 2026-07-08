using System.Collections.Generic;
using UnityEngine;

public class BulletPoolHandler : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletsParent;
    public int poolSize = 20;

    private static Queue<GameObject> pool = new();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bulletsParent.transform);
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
            bullet.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            return bullet;
        }
        else
        {
            Debug.Log("Need to expand bullet pool");
            GameObject bullet = Instantiate(bulletPrefab);
            //bullet.transform.SetParent(bulletsParent.transform);
            bullet.SetActive(true);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ReturnBullet(gameObject);
    }
    */

}
