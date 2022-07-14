using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float size = 2f;
    public float minSize = 1.2f;
    public float maxSize = 3f;
    public float speed = 20.0f;
    public float lifetime = 30.0f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.color = Color.gray;
        //randomize rotation of sprite
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        _rb.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rb.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.lifetime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //split
            if (this.size / 2 >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<AsteroidsManager>().AsteroidDestroyed(this); //find better way
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f; //random for small
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
