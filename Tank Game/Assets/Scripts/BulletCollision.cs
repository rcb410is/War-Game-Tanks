using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    [SerializeField] GameObject bullet;
    [SerializeField] MeshRenderer bulletModel;
    [SerializeField] BulletPoolHandler bulletPoolHandler;

    void RemoveBullet()
    {
        bulletModel.GetComponent<MeshRenderer>().enabled = true;
        bullet.GetComponent<Collider>().enabled = true;
        bulletPoolHandler.ReturnBullet(bullet);
    }

    void OnCollisionEnter(Collision collision)
    {
        var particle = bullet.GetComponent<ParticleSystem>();
        particle.Play();
        bulletModel.GetComponent<MeshRenderer>().enabled = false;
        bullet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        bullet.GetComponent<Collider>().enabled = false;
        Invoke("RemoveBullet", 2f);
    }
}
