using UnityEngine.InputSystem;
using UnityEngine;

public class Interact : MonoBehaviour
{
    internal static GameObject selected;

    [SerializeField] UiHandler uiHandler;

    Interactable hoveredObject;

    void Start()
    {
    }
    Vector3 gizCenter;
    void FixedUpdate()
    {
        if (!selected)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit giz, 3, LayerMask.GetMask("Highlight", "Interactable")))
                gizCenter = giz.point;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3, LayerMask.GetMask("Highlight", "Interactable")) && hit.collider.TryGetComponent(out Interactable obj))
            {
                hoveredObject = obj;
                if (!obj.Highlighted)
                    obj.Highlight();
            }
            else if (hoveredObject != null)
            {
                hoveredObject.UnHighlight();
                hoveredObject = null;
            }
        }
    }

    void OnSelect(InputValue iv)
    {
        if (iv.isPressed && UiHandler.uiStack.Count == 0)
            if (hoveredObject != null)
                SelectTheHovered();
            else
                selected = null;
    }
    void OnBack(InputValue iv)
    {
        if (iv.isPressed)
        {
            if (UiHandler.uiStack.Count > 0)
                uiHandler.Back();
            else
                uiHandler.Pause();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gizCenter, 0.1f);
    }
    void SelectTheHovered()
    {
        uiHandler.ShowToolbar();
        selected = hoveredObject.gameObject;
        Rigidbody rb = hoveredObject.GetComponent<Rigidbody>();
        if (!rb.isKinematic)
            rb.velocity = Vector3.zero;
        rb.Sleep();
        hoveredObject.UnHighlight();
        hoveredObject = null;
    }
}
