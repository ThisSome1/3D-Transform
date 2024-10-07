using System.Linq;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    CharachterMovement charachterMovement;
    void Start()
    {
        charachterMovement = GetComponentInChildren<CharachterMovement>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rb) && collision.gameObject.GetComponent<Interactable>() && charachterMovement.Moving)
        {
            Vector3 cp = Vector3Ext.Average(collision.contacts.Select((x) => x.point));
            rb.AddForce((cp.XOZ() - transform.position.XOZ()).normalized * charachterMovement.Speed / 10, ForceMode.Impulse);
        }
    }
}
