using UnityEngine;
using UnityEngine.InputSystem;

public class CharachterMovement : MonoBehaviour
{
    [SerializeField] float speed = 5, lookSensitivity = 4;

    Vector3 movement;

    void Start()
    {
    }

    void Update()
    {
        transform.parent.position += (movement.x * transform.right.xoz().normalized + movement.z * transform.forward.xoz().normalized) * Time.deltaTime * speed;

        if (UiHandler.uiStack.Count > 0 && enabled)
        {
            enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnMove(InputValue iv)
    {
        if (!enabled) return;
        Vector2 val = iv.Get<Vector2>();
        movement.x = val.x;
        movement.z = val.y;
    }

    void OnLook(InputValue iv)
    {
        if (!enabled) return;
        Vector2 val = iv.Get<Vector2>();
        transform.parent.Rotate(Vector3.up, val.x * lookSensitivity / 10, Space.World);
        transform.Rotate(Vector3.right, -val.y * lookSensitivity / 10, Space.Self);
    }

}
