using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    internal bool Highlighted => highlighted;

    bool highlighted;

    void Start()
    {
    }

    void Update()
    {
    }

    internal void Highlight()
    {
        highlighted = true;
        gameObject.layer = LayerMask.NameToLayer("Highlight");

    }
    internal void UnHighlight()
    {
        highlighted = false;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}
