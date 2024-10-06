using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [SerializeField] AnimatedRectTransform mainMenu, addMenu, confirmMenu, toolbarMenu, toolName;

    [SerializeField] GameObject prefabCube, prefabCylinder, prefabSphere;
    [SerializeField] CharachterMovement movementScript;
    [SerializeField] Material defaultMaterial;

    void Start()
    {
        Invoke("Pause", 0.01f);
    }
    void Update()
    {

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
        toolName.GetComponentInChildren<TextMeshProUGUI>().text = "Tool:\n" + tool.ToString();
        toolbarMenu.MoveAnimatedTo(new(-192, 0));
        toolName.AlphaAnimatedTo(1);
        uiStack.Push(toolName);
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
