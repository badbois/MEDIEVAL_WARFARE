using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private Renderer _renderer;
    private RuneManager _rune_manager;

    [Header("Health & score")]
    public float shield = 0f;
    public float max_health = 100f;
    public float health = 100f;
    // Temps pendant lequel le personnage est invincible apr�s avoir �t� touch� (en secondes)
    public float invulnerability_time = 2.0f;
    
    public int nb_coins = 0;
    [SerializeField] private TextMeshProUGUI health_text;
    [SerializeField] private TextMeshProUGUI coins_text;
    

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rune_manager = GetComponent<RuneManager>();
        health = max_health;
        Debug.Log("Start !");
    }

    private void Update()
    {
        if (is_dead() && GameManager.Instance._state == GameState.RUNNING)
        {
            Debug.Log("Player died");
            StopCoroutine("invulnerability");
            GameManager.Instance.set_state(GameState.FAIL_MENU);
        }
    }

    public void Init(float rune_health, float rune_shield)
    {
        // Update Player stats according to equiped runes
        max_health *= rune_health;
        health = max_health;
        shield = rune_shield;
        health_text.SetText("HEALTH : " + health);
        coins_text.SetText("MONEY : " + nb_coins);
    }

    //The layer 8 is the player, the 9th is the enemy
    IEnumerator invulnerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Color player_sprite_color = _renderer.material.color;
        player_sprite_color.a = 0.5F;
        _renderer.material.color = player_sprite_color;

        yield return new WaitForSeconds(invulnerability_time);

        Physics2D.IgnoreLayerCollision(8, 9, false);
        player_sprite_color.a = 1.0F;
        _renderer.material.color = player_sprite_color;
    }

    public bool is_dead()
    {
        return health <= 0;
    }

    public void die()
    {
        health = 0.0F;
    }

    public void take_damages(float damages)
    {
        if (shield == 0)
        {
            // Invincibility for 2 seconds
            StartCoroutine("invulnerability");
            health = Mathf.Clamp(health - damages, 0, max_health);
            health_text.SetText("HEALTH : " + health);
        }
        else
        {
            shield--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si le collider en contact est l'ennemi
        if (collision.transform.TryGetComponent<Enemy>(out Enemy enemy))
        {
            take_damages(enemy.get_damage());
        }
    }
}

