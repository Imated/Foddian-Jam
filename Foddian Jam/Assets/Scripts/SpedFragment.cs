using UnityEngine;

public class SpedFragment : MonoBehaviour
{
    [SerializeField] private float speeeeeeeeeeeeedMult = 1.1f;
    
    public void Go(Vector2 direction, float magVel)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * magVel * 0.8f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Rigidbody2D>().velocity *= speeeeeeeeeeeeedMult;
            Destroy(gameObject);
        }
    }
}
