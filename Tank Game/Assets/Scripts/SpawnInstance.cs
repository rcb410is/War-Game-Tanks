using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpawnInstance : MonoBehaviour
{

    public static GameObject tank;
    public static GameObject selectionIndicator;
    [SerializeField] Button spawnButton;
    public static Text buttonText;
    public static bool isSpawnActivated = false;
    public static bool canInteractWithButtons = true;

    public void SpawnTank(GameObject newTank)
    {
        tank = newTank;
        isSpawnActivated = true;
        canInteractWithButtons = false;
    }

    public void SpawnSelection(GameObject newSelectionIndicator)
    {
        selectionIndicator = newSelectionIndicator;
        selectionIndicator = Instantiate(selectionIndicator, new Vector3(0, -45, 0), Quaternion.identity);
    }

    public void AssignText(Text currentButtonText)
    {
        buttonText = currentButtonText;
        buttonText.text = "Press 'E' to Confirm";
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

        if (isSpawnActivated && Keyboard.current.eKey.isPressed)
        {
            Debug.Log("Preparing to spawn");
            Instantiate(tank, hit.point, Quaternion.identity);
            isSpawnActivated = false;
            canInteractWithButtons = true;
            buttonText.text = tank.name;
            selectionIndicator.transform.position = new Vector3(0, -45, 0);
        }

    }

}