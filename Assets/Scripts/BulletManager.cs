using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float damage;
    private Rigidbody rb;

    public float Damage => damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
