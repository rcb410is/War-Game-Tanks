using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour
{

    static GameObject tank;
    [SerializeField] SpawnInstance spawnInstance;
    [SerializeField] MouseControl mouseControl;
    [SerializeField] MultiplayerHandler multiplayerHandler;
    [SerializeField] Transform pointer;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] Transform radiusIndicator;
    [SerializeField] Transform tankDeadZone;
    [SerializeField] Transform indicatorsParent;
    [SerializeField] Button shootButton;
    static bool isAnyTankSelected;
    bool initialSelectionWasMade;
    bool didRaycastHit;
    string currentPlayerTag;
    RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointer = Instantiate(pointer, new Vector3(0, -40, 0), Quaternion.identity);
        selectionIndicator = Instantiate(selectionIndicator, new Vector3(0, -45, 0), Quaternion.identity);
        radiusIndicator = Instantiate(radiusIndicator, new Vector3(0, -50, 0), Quaternion.identity);
        tankDeadZone = Instantiate(tankDeadZone, new Vector3(0,-55, 0), Quaternion.identity);

        pointer.SetParent(indicatorsParent.transform);
        radiusIndicator.SetParent(indicatorsParent.transform);
        selectionIndicator.SetParent(indicatorsParent.transform);
        tankDeadZone.SetParent(indicatorsParent.transform);

    }

    public GameObject GetPlayer() { return tank; }

    public bool IsSelected() { return isAnyTankSelected; }
    
    public bool SelectFirstTank()
    {
        if (didRaycastHit && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (hit.collider.CompareTag(currentPlayerTag/*"Player"*/)) {
                tank = hit.collider.gameObject;
                initialSelectionWasMade = true;
                isAnyTankSelected = true;
                //Debug.Log("First tank selected");
            }
        }
        return isAnyTankSelected;
    }

    bool SelectTank()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (didRaycastHit)
            {
                if (hit.collider.Equals(tank.GetComponent<Collider>()) && !isAnyTankSelected && hit.collider.CompareTag(currentPlayerTag))
                {
                    //Debug.Log($"Selected {player}");
                    isAnyTankSelected = true;
                    selectionIndicator.position = tank.transform.position;
                    radiusIndicator.position = tank.transform.position;
                }
                else if (hit.collider.Equals(tank.GetComponent<Collider>()) && isAnyTankSelected && hit.collider.CompareTag(currentPlayerTag))
                {
                    //Debug.Log($"Deselected {player}");
                    isAnyTankSelected = false;
                    selectionIndicator.position = new Vector3(0, -20, 0);
                    radiusIndicator.position = new Vector3(0, -30, 0);
                    pointer.position = new Vector3(0, -10, 0);
                }
                else if (hit.collider.CompareTag(currentPlayerTag) || hit.collider.CompareTag("Button"))
                {
                    //Debug.Log("Selected other tank");
                    //Debug.Log($"Selected {hit.collider.name}");
                    isAnyTankSelected = true;
                    tank = hit.collider.gameObject;
                    selectionIndicator.position = tank.transform.position;
                    radiusIndicator.position = tank.transform.position;
                    pointer.position = new Vector3(0, -10, 0);
                }
                else
                {
                    //Debug.Log("Did Not Select");
                }
                
            }

        }
        return isAnyTankSelected;

    }

    // Update is called once per frame
    void Update()
    {
        hit = mouseControl.GetHit();
        didRaycastHit = mouseControl.DidRaycastHit();

        currentPlayerTag = multiplayerHandler.GetCurrentPlayerTag();

        if (Mouse.current.leftButton.wasPressedThisFrame && (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("PlayerDeadZone")) && isAnyTankSelected)
        {
            pointer.position = hit.point;
        }

        if (!initialSelectionWasMade)
        {
            isAnyTankSelected = SelectFirstTank();
        }
        else if (tank.GetComponent<ShootBullet>().IsInShootMode())
        {
            radiusIndicator.position = new Vector3(0, -30, 0);
            selectionIndicator.SetPositionAndRotation(tank.transform.position, tank.transform.rotation);
            tankDeadZone.position = tank.transform.position;
            //Debug.Log("Can't select, tank is in shoot mode");
        }
        else
        {
            isAnyTankSelected = SelectTank();
        }

        if (isAnyTankSelected && !tank.GetComponent<ShootBullet>().IsInShootMode())
        {
            selectionIndicator.SetPositionAndRotation(tank.transform.position, tank.transform.rotation);
            radiusIndicator.position = tank.transform.position;
            tankDeadZone.position = new Vector3(0, -55, 0);
        }

        if (spawnInstance.IsSpawnActivated())
        {
            isAnyTankSelected = false;
            selectionIndicator.position = new Vector3(0, -20, 0);
            radiusIndicator.position = new Vector3(0, -30, 0);
            pointer.position = new Vector3(0, -10, 0);
        }
        
        if (initialSelectionWasMade)
        {
            if (tank.tag != currentPlayerTag)
            {
                isAnyTankSelected = false;
                selectionIndicator.position = new Vector3(0, -20, 0);
                radiusIndicator.position = new Vector3(0, -30, 0);
            }
        }

        if (!isAnyTankSelected)
        {
            shootButton.interactable = false;
        }
        else
        {
            shootButton.interactable = true;
        }

    }

}
