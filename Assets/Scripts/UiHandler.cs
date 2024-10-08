using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using System.Linq;

public class UiHandler : MonoBehaviour
{
    public enum Tools : int
    {
        None = 0,
        Move = 1,
        Scale = 2,
        Rotate = 3,
    }
    internal static Stack<AnimatedRectTransform> uiStack = new();
    internal static Tools tool = Tools.None;

    [SerializeField] AnimatedRectTransform mainMenu, addMenu, confirmMenu, toolbarMenu, toolName, crosshair;

    [SerializeField] GameObject prefabCube, prefabCylinder, prefabSphere;
    [SerializeField] CharachterMovement movementScript;

    EventSystem eventSystem;
    Vector2 currMousePos, prevMousePos;
    bool clickDown = false, prevClickDown = false, dragging = false, rclickDown = false, screenDragging = false;
    bool selectedObjectWasKinematic = false;

    TextMeshProUGUI fps;
    readonly List<int> fpsCache = new();

    void Start()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
        fps = GameObject.Find("FPS").GetComponent<TextMeshProUGUI>();
        Invoke("Pause", 0.01f);
    }
    void Update()
    {
        fpsCache.Add((int)(1 / Time.deltaTime));
        if (fpsCache.Count > 10) fpsCache.RemoveAt(fpsCache.Count - 1);
        int avgFps = (int)fpsCache.Average();
        fps.text = avgFps.ToString();
        if (avgFps > 60)
            fps.color = Color.green;
        else if (avgFps > 45)
            fps.color = Color.Lerp(Color.yellow, Color.green, Mathf.Clamp01((avgFps - 45) / 15f));
        else
            fps.color = Color.Lerp(Color.red, Color.yellow, Mathf.Clamp01((avgFps - 30) / 15f));

        Vector2 delta = currMousePos - prevMousePos;
        if (dragging)
        {
            switch (tool)
            {
                case Tools.Move:
                    Vector3 dif = Camera.main.ScreenToWorldPoint(new(currMousePos.x, currMousePos.y, 3)) - Camera.main.ScreenToWorldPoint(new(prevMousePos.x, prevMousePos.y, 3));
                    Interact.selected.transform.position += dif;
                    break;
                case Tools.Scale:
                    Interact.selected.transform.localScale += delta.y * Vector3.one / Screen.dpi;
                    break;
                case Tools.Rotate:
                    Interact.selected.transform.Rotate(movementScript.transform.up, -delta.x, Space.World);
                    Interact.selected.transform.Rotate(movementScript.transform.right, delta.y, Space.World);
                    break;
            }
        }
        if (screenDragging)
        {
            movementScript.transform.parent.Rotate(Vector3.up, delta.x / Screen.dpi * 5, Space.World);
            movementScript.transform.Rotate(Vector3.right, -delta.y / Screen.dpi * 5, Space.Self);
        }
        prevMousePos = currMousePos;

        if (uiStack.Count > 0) { if (crosshair.gameObject.activeSelf) crosshair.gameObject.SetActive(false); }
        else if (!crosshair.gameObject.activeSelf) crosshair.gameObject.SetActive(true);
    }
    void FixedUpdate()
    {
        if (Interact.selected && clickDown)
        {
            Debug.DrawRay(movementScript.transform.position, (Camera.main.ScreenToWorldPoint(new(currMousePos.x, currMousePos.y, 3)) - movementScript.transform.position).normalized * 10, Color.blue);
            if (tool == Tools.Move)
            {
                if (Physics.Raycast(movementScript.transform.position, Camera.main.ScreenToWorldPoint(new(currMousePos.x, currMousePos.y, 3)) - movementScript.transform.position, out RaycastHit hit, 10, LayerMask.GetMask("Interactable"))
                    && hit.collider.TryGetComponent(out Interactable obj) && obj.gameObject == Interact.selected && !prevClickDown)
                    dragging = true;
            }
            else if (!eventSystem.IsPointerOverGameObject() && !prevClickDown)
                dragging = true;
        }
        else if (dragging)
            dragging = false;

        if (Interact.selected && !clickDown && rclickDown)
            screenDragging = true;
        else if (screenDragging)
            screenDragging = false;

        prevClickDown = clickDown;
    }

    void OnClick(InputValue iv)
    {
        if (uiStack.Count == 0) return;
        clickDown = iv.isPressed;
        prevMousePos = currMousePos;
    }
    void OnRightClick(InputValue iv)
    {
        if (uiStack.Count == 0) return;
        rclickDown = iv.isPressed;
        prevMousePos = currMousePos;
    }
    void OnPoint(InputValue iv)
    {
        if (uiStack.Count == 0) return;
        currMousePos = iv.Get<Vector2>();
    }
    void OnToolSelect(InputValue iv)
    {
        if (uiStack.Count == 0) return;
        float val = iv.Get<float>();
        if (val == 4)
        {
            if (uiStack.Peek() == toolbarMenu) ToggleRigid();
        }
        else if (uiStack.Peek() == toolbarMenu || uiStack.Peek() == toolName)
            SelectTool(val switch { 1 => "move", 2 => "scale", 3 => "rotate", _ => "" });
    }

    internal void ShowToolbar()
    {
        toolbarMenu.MoveAnimatedTo(new(0, 0));
        uiStack.Push(toolbarMenu);
    }

    public void Back()
    {
        if (uiStack.Count > 0)
        {
            AnimatedRectTransform top = uiStack.Pop();
            if (top == toolbarMenu)
                top.MoveAnimatedTo(new(-192, 0));
            else if (top == toolName)
            {
                tool = Tools.None;
                Rigidbody rb = Interact.selected.GetComponent<Rigidbody>();
                rb.isKinematic = selectedObjectWasKinematic;
                top.AlphaAnimatedTo(0);
                toolbarMenu.MoveAnimatedTo(new(0, 0));
            }
            else
                top.ScaleAnimatedTo(new(0, 0, 0));
            if (uiStack.Count == 0)
                Play();
        }
    }
    public void SelectTool(string name)
    {
        switch (name)
        {
            case "move":
                tool = Tools.Move;
                break;
            case "scale":
                tool = Tools.Scale;
                break;
            case "rotate":
                tool = Tools.Rotate;
                break;
            default:
                return;
        }
        Rigidbody rb = Interact.selected.GetComponent<Rigidbody>();
        selectedObjectWasKinematic = rb.isKinematic;
        rb.isKinematic = true;
        toolName.GetComponentInChildren<TextMeshProUGUI>().text = "Tool:\n" + tool.ToString();
        toolbarMenu.MoveAnimatedTo(new(-192, 0));
        toolName.AlphaAnimatedTo(1);
        if (uiStack.Peek() != toolName)
            uiStack.Push(toolName);
        prevClickDown = true;
    }
    public void ToggleRigid()
    {
        Rigidbody rb = Interact.selected.GetComponent<Rigidbody>();
        rb.isKinematic = !rb.isKinematic;
    }
    public void DeleteObject()
    {
        Destroy(Interact.selected);
        Interact.selected = null;
        Back();
    }
    public void Play()
    {
        mainMenu.ScaleAnimatedTo(new(0, 0, 0));
        uiStack.Clear();
        Cursor.lockState = CursorLockMode.Locked;
        movementScript.enabled = true;
        Cursor.visible = false;
        Interact.selected = null;
    }
    public void Pause()
    {
        mainMenu.ScaleAnimatedTo(new(1, 1, 1));
        uiStack.Push(mainMenu);
    }
    public void AddMenu()
    {
        addMenu.ScaleAnimatedTo(new(1, 1, 1));
        uiStack.Push(addMenu);
    }
    public void Exit()
    {
        uiStack.Push(confirmMenu);
        confirmMenu.ScaleAnimatedTo(new(1, 1, 1));
    }
    public void AddPrimitive(string name)
    {
        Vector3 pos = movementScript.gameObject.transform.position + movementScript.gameObject.transform.forward * 2;
        if (pos.y < 1) pos.y = 1;
        switch (name)
        {
            case "cube":
                Instantiate(prefabCube, pos, Quaternion.LookRotation(movementScript.gameObject.transform.forward));
                break;
            case "cylinder":
                Instantiate(prefabCylinder, pos, Quaternion.LookRotation(movementScript.gameObject.transform.forward));
                break;
            case "sphere":
                Instantiate(prefabSphere, pos, Quaternion.LookRotation(movementScript.gameObject.transform.forward));
                break;
            default:
                return;
        }
        while (uiStack.Count > 0)
            Back();
    }
    public void Confirm()
    {
        uiStack.Pop();
        confirmMenu.ScaleAnimatedTo(new(0, 0, 0));
        AnimatedRectTransform top = uiStack.Peek();
        if (top == toolbarMenu)
            Destroy(Interact.selected);
        else
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
