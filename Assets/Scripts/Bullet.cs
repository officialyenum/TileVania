using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float bulletDamage = 1f;
    PlayerMovementScript player;
    float xSpeed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovementScript>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyMovement enemyDamage = other.gameObject.GetComponent<EnemyMovement>();
            enemyDamage.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
