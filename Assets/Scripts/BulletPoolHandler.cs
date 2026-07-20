using System.Collections.Generic;
using UnityEngine;

public class BulletPoolHandler : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletsParent;
    public int poolSize = 20;

    static Queue<GameObject> pool = new();

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ObjectPool");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bulletsParent.transform);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
        DontDestroyOnLoad(bulletsParent);

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

}
