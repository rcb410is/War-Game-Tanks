using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour
{

    static GameObject tank;
    [SerializeField] SpawnInstance spawnInstance;
    [SerializeField] Transform pointer;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] Transform radiusIndicator;
    [SerializeField] Button shootButton;
    static bool isSelected;
    bool initialSelectionWasMade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointer = Instantiate(pointer, new Vector3(0, -40, 0), Quaternion.identity);
        selectionIndicator = Instantiate(selectionIndicator, new Vector3(0, -45, 0), Quaternion.identity);
        radiusIndicator = Instantiate(radiusIndicator, new Vector3(0, -50, 0), Quaternion.identity);
    }

    public Transform GetPoint() { return pointer; }

    public Transform GetSelectionIndicator() { return selectionIndicator; }

    public Transform GetRadiusIndicator() { return radiusIndicator; }

    public GameObject GetPlayer() { return tank; }

    public bool IsSelected() { return isSelected; }
    
    public bool SelectFirstTank()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(rayOrigin, out RaycastHit hit) && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (hit.collider.CompareTag("Player")) {
                tank = hit.collider.gameObject;
                initialSelectionWasMade = true;
                isSelected = true;
                //Debug.Log("First tank selected");
            }
        }
        return isSelected;
    }

    bool SelectTank()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(rayOrigin, out RaycastHit hit))
            {
                if (hit.collider.Equals(tank.GetComponent<Collider>()) && !isSelected)
                {
                    //Debug.Log("Selected " + player);
                    isSelected = true;
                    selectionIndicator.position = tank.transform.position;
                    radiusIndicator.position = tank.transform.position;
                }
                else if (hit.collider.Equals(tank.GetComponent<Collider>()) && isSelected)
                {
                    //Debug.Log("Deselected " + player);
                    isSelected = false;
                    selectionIndicator.position = new Vector3(0, -20, 0);
                    radiusIndicator.position = new Vector3(0, -30, 0);
                    pointer.position = new Vector3(0, -10, 0);
                }
                else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Button"))
                {
                    //Debug.Log("Selected other tank");
                    isSelected = true;
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
        return isSelected;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(rayOrigin, out RaycastHit hit))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && hit.collider.CompareTag("Ground") && isSelected)
            {
                pointer.position = hit.point;
            }

        }

        if (!initialSelectionWasMade)
        {
            isSelected = SelectFirstTank();
        }
        else if (tank.GetComponent<ShootBullet>().IsAimReady())
        {
            radiusIndicator.position = new Vector3(0, -30, 0);
            selectionIndicator.SetPositionAndRotation(tank.transform.position, tank.transform.rotation);
            //Debug.Log("Can't select, tank is in shoot mode");
        }
        else
        {
            isSelected = SelectTank();
        }

        if (isSelected && !tank.GetComponent<ShootBullet>().IsAimReady())
        {
            selectionIndicator.SetPositionAndRotation(tank.transform.position, tank.transform.rotation);
            radiusIndicator.position = tank.transform.position;
        }

        if (spawnInstance.IsSpawnActivated())
        {
            isSelected = false;
            selectionIndicator.position = new Vector3(0, -20, 0);
            radiusIndicator.position = new Vector3(0, -30, 0);
            pointer.position = new Vector3(0, -10, 0);
        }

        if (!isSelected)
        {
            shootButton.interactable = false;
        }
        else
        {
            shootButton.interactable = true;
        }

    }

}
