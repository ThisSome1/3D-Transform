using System.Linq;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    CharachterMovement charachterMovement;
    void Start()
    {
        charachterMovement = GetComponentInChildren<CharachterMovement>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb) && other.gameObject.GetComponent<Interactable>())
        {
            if (charachterMovement.Moving)
                rb.AddForce((other.transform.position - transform.position).XOZ().normalized * charachterMovement.Speed / 10, ForceMode.Impulse);
            else
                rb.AddForce(Vector3.Project(-rb.linearVelocity, (other.transform.position - transform.position).normalized.XOZ()) * 2, ForceMode.VelocityChange);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            if (charachterMovement.Moving)
                rb.AddForce((other.transform.position - transform.position).XOZ().normalized * charachterMovement.Speed / 10, ForceMode.Impulse);
            else
                rb.AddForce((other.transform.position - transform.position).normalized * 2, ForceMode.VelocityChange);
        }
    }
}
