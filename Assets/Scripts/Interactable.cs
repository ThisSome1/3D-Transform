using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    internal bool Highlighted => highlighted;

    [SerializeField] float gravity = 9.807f;

    bool highlighted;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!rigidBody.isKinematic)
            rigidBody.velocity -= new Vector3(0, gravity, 0) * Time.deltaTime;
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
