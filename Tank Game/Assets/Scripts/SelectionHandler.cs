using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionHandler : MonoBehaviour
{

    static GameObject player;
    [SerializeField] Transform pointer;
    [SerializeField] Transform selectionIndicator;
    [SerializeField] Transform radiusIndicator;
    static bool isSelected;
    bool wasInitialSelectionMade;

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

    public GameObject GetPlayer() { return player; }

    public bool IsSelected() { return isSelected; }
    
    public bool SelectFirstTank()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(rayOrigin, out RaycastHit hit) && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (hit.collider.CompareTag("Player")) {
                player = hit.collider.gameObject;
                wasInitialSelectionMade = true;
                isSelected = true;
                Debug.Log("First tank selected");
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
                if (hit.collider.Equals(player.GetComponent<Collider>()) && !isSelected)
                {
                    isSelected = true;
                    //Debug.Log("Selected " + player);
                    selectionIndicator.position = player.transform.position;
                    radiusIndicator.position = player.transform.position;
                }
                else if (hit.collider.Equals(player.GetComponent<Collider>()) && isSelected)
                {
                    isSelected = false;
                    //Debug.Log("Deselected " + player);
                    selectionIndicator.position = new Vector3(0, -20, 0);
                    radiusIndicator.position = new Vector3(0, -30, 0);
                    pointer.position = new Vector3(0, -10, 0);
                }
                else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Button"))
                {
                    isSelected = true;
                    player = hit.collider.gameObject;
                    //Debug.Log("Selected other tank");
                    selectionIndicator.position = player.transform.position;
                    radiusIndicator.position = player.transform.position;
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

        if (!wasInitialSelectionMade)
        {
            isSelected = SelectFirstTank();
        }
        else
        {
            isSelected = SelectTank();
        }

        if (isSelected)
        {
            selectionIndicator.rotation = player.transform.rotation;
            selectionIndicator.position = player.transform.position;
            radiusIndicator.position = player.transform.position;
        }
    }


}
