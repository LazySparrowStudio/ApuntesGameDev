using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MovementController movementController;
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }
    }
}
