using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 2f;
    //public float bullet_size = 1f;

    // Start is called before the first frame update
    void Start()
    {
        speed *= RuneManager.Instance.bullet_speed_rune;
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
            damage *= RuneManager.Instance.damage_rune;
            if(Random.Range(0, 100) < RuneManager.Instance.critial_hit_rune){
                damage *= 2f;
                Debug.Log("Critical hit " + damage);
            }
            enemy.take_damages(damage);
        }
    }
}
