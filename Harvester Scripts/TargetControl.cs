using UnityEngine;

public class TargetControl : MonoBehaviour
{
    private GameObject player;
    private bool isControllerConnected;
    Vector3 controllerDirection;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controllerDirection = Vector3.left;
    }

    void Update()
    {
        if (Input.GetJoystickNames().Length > 1){
            isControllerConnected = true;
        }
        else{
            isControllerConnected = false;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 newPosition = new Vector3(mousePosition.x, mousePosition.y, 0f);

        Vector3 mouseDirection = Vector3.Normalize(newPosition - player.transform.position);

        Vector3 direction;
        if (isControllerConnected)
        {
            Vector3 controllerInput = new Vector3(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"), 0);
            if (controllerInput.magnitude > 0.5f)
            {
                controllerDirection = controllerInput.normalized;
            }

            direction = controllerDirection;
        }
        else
        {
            direction = mouseDirection;
        }

        transform.position = player.transform.position + direction * 10;

        // chooses correct facing direction
        player.GetComponent<PlayerMovement>().SetFacingDirection(direction);
    }
}