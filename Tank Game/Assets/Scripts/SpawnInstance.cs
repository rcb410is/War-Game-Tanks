using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpawnInstance : MonoBehaviour
{

    public static GameObject tank;
    public static GameObject selectionIndicator;
    public static Text buttonText;
    public static bool spawnActivated = false;

    public void SpawnTank(GameObject newTank)
    {
        tank = newTank;
        spawnActivated = true;
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
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(rayOrigin, out RaycastHit hit) && hit.collider.CompareTag("Ground") && spawnActivated)
        {
            selectionIndicator.transform.position = hit.point;
        }

        if (spawnActivated && Keyboard.current.eKey.isPressed)
        {
            Debug.Log("Preparing to spawn");
            Instantiate(tank, hit.point, Quaternion.identity);
            spawnActivated = false;
            buttonText.text = tank.name;
            selectionIndicator.transform.position = new Vector3(0, -45, 0);
        }

    }

}