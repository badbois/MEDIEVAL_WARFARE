using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 2f;

    // Start is called before the first frame update
    void Start()
    {
        speed *= Runes.bullet_speed_rune;
        rb.velocity = transform.right * speed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Debug.Log(hitInfo.name);
        Destroy(gameObject);
        Ennemy enemy = hitInfo.GetComponent<Ennemy>();
        if (enemy != null)
        {
            enemy.take_damages(damage * Runes.damage_rune);
        }
    }
}
