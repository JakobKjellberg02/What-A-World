using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public InputAction cameraControl;

    Vector2 moveDir = Vector2.zero;

    private void OnEnable()
    {
        cameraControl.Enable();
    }

    private void OnDisable()
    {
        cameraControl.Disable();
    }


    void Update()
    {
        moveDir = cameraControl.ReadValue<Vector2>();
        Vector3 move = new(moveDir.x, 0, moveDir.y);
        transform.position += move * speed * Time.deltaTime;
    }
}
