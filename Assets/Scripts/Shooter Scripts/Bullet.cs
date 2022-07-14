using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed = 500.0f;
    public float lifetime = 10.0f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _rb.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.lifetime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
