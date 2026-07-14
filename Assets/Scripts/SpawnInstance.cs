using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public enum TanksTypes
{
    _9TP, _KV2
}

public class SpawnInstance : MonoBehaviour
{

    public static GameObject tank;

    [SerializeField] GameObject selectionIndicator;
    [SerializeField] Transform tanksParent;
    [SerializeField] Transform indicatorsParent;
    [SerializeField] Button spawnButton;
    [SerializeField] TanksTypes targetedSpawnType;
    public static Text buttonText;
    public static bool isSpawnActivated = false;
    public static bool canInteractWithButtons = true;
    public int poolSize = 10;

    static Queue<GameObject> pool = new();

     void Start()
    {
        selectionIndicator = Instantiate(selectionIndicator, new Vector3(0, -45, 0), Quaternion.identity);
        selectionIndicator.transform.SetParent(indicatorsParent);
    }

    public void ReturnTank(GameObject tank)
    {
        tank.SetActive(false);
        pool.Enqueue(tank);
    }

    public bool IsSpawnActivated() { return isSpawnActivated; }

    public void SpawnTank(GameObject newTank)
    {
        tank = newTank;
        if (!tank.GetComponent<ShootBullet>().IsInShootMode())
        {
            isSpawnActivated = true;
            canInteractWithButtons = false;
        }
        
    }

    public void AssignText(Text currentButtonText)
    {
        if (!tank.GetComponent<ShootBullet>().IsInShootMode())
        {
            buttonText = currentButtonText;
            buttonText.text = "Click to confirm";
        }
        
    }

    void Update()
    {
        if (!canInteractWithButtons)
        {
            spawnButton.interactable = false;
        }
        else
        {
            spawnButton.interactable = canInteractWithButtons;
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(rayOrigin, out RaycastHit hit) && hit.collider.CompareTag("Ground") && isSpawnActivated)
        {
            selectionIndicator.transform.position = hit.point;
        }

        if (isSpawnActivated && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (pool.Count > 0)
            {
                GameObject queuedTank = pool.Dequeue();
                var playerMovement = queuedTank.GetComponent<PlayerMovement>();
                var tankType = playerMovement.TankType;
                Debug.Log($"[{name}]: Pool count above 0 {tankType} {targetedSpawnType}");
                int count = 0;
                while (tankType != targetedSpawnType && count != pool.Count)
                {
                    //pool.Enqueue(queuedTank);
                    //Debug.Log("Not a match, requeued tank");
                    pool.Enqueue(queuedTank);
                    count++;
                    pool.Dequeue();
                    Debug.Log($"There is a tank of type {tank} in the pool");
                }
                if (tankType == targetedSpawnType)
                {
                    queuedTank.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    queuedTank.transform.SetPositionAndRotation(selectionIndicator.transform.position, Quaternion.identity);
                    queuedTank.SetActive(true);
                }
                else
                {
                    var created = Instantiate(tank, selectionIndicator.transform.position, Quaternion.identity);
                    created.transform.SetParent(tanksParent);
                    created.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    created.transform.SetPositionAndRotation(selectionIndicator.transform.position, Quaternion.identity);
                    created.SetActive(true);
                }

            }
            else
            {
                var created = Instantiate(tank, selectionIndicator.transform.position, Quaternion.identity);
                created.transform.SetParent(tanksParent);
                created.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                created.transform.SetPositionAndRotation(selectionIndicator.transform.position, Quaternion.identity);
                created.SetActive(true);
            }
            isSpawnActivated = false;
            canInteractWithButtons = true;
            buttonText.text = tank.name;
        }

        if (!isSpawnActivated)
        {
            selectionIndicator.transform.position = new Vector3(0, -45, 0);
        }

    }

}