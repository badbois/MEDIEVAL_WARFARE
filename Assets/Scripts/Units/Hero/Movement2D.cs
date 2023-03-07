using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement2D : MonoBehaviour
{

    [Header("Movement Variables")]
    [SerializeField] private float _movement_acceleration = 50f;
    [SerializeField] private float _max_move_speed = 12f;
    [SerializeField] private float _ground_linear_drag = 10f; //a.k.a deceleration
    [SerializeField] private float _default_speed = 7f;
    private bool _changing_direction;
    private bool _is_crouching = false;

    [Header("jump Variables")]
    [SerializeField] private float _jump_force = 12f;
    [SerializeField] private float _air_linear_drag = 2.5f;
    [SerializeField] private float _fall_gravity = 8f;
    [SerializeField] private float _low_jump_fall_gravity = 5f;
    [SerializeField] private int _extra_jumps = 1;
    [SerializeField] private float _fast_fall_gravity = 500f; 
	[SerializeField] private float _fastfall_max_speed = 50.0f;
    private int _extra_jumps_count = 0;
    private bool _can_jump = false;
    private bool _on_ground = true;

    [Header("Ground Collision Variable")]
    [SerializeField] private float _ground_raycast_length = 0.8f;
    [SerializeField] private Vector3 _ground_raycast_offset = new Vector3(.3f, 0f, 0f);
    [SerializeField] private LayerMask _ground_layer;


    private PlayerData _player;
    private float _horizontal_direction;
    private float _vertical_direction;
    public Animator animator;
    bool facingRight = true;
    private BoxCollider2D boxCollider;


    private void Awake()
    { 
        _player = gameObject.GetComponent(typeof(PlayerData)) as PlayerData;
        boxCollider = GetComponent<BoxCollider2D>();
        _movement_acceleration *= Runes.speed_rune;
        _jump_force *= Runes.high_jump_rune;
    }

    private void Update()
    {
        boxCollider.size = new Vector2(0.39f, 0.97f);
        boxCollider.offset = new Vector2(0.09f, -0.1f);

        _horizontal_direction = get_input().x;
        _vertical_direction = get_input().y;

        
        if (_horizontal_direction > 0 && !facingRight)
        {
            //Les "Flip" et "FlipSword" sont � enlever pour le niveau mais � garder pour le boss !
            //Flip();
            animator.speed = 2.0f;
            animator.SetBool("IsRunning",true);
        }

        if (_horizontal_direction == 0)
        {
            animator.SetBool("IsRunning", false);
            animator.speed = 1.0f;
        }
        if (_horizontal_direction < 0 && facingRight)
        {
            /*Flip();
            FlipSword();*/
            animator.speed = 0.5f;
            animator.SetBool("IsRunning", true);
        }

        _changing_direction = (_player.rb.velocity.x > 0f && _horizontal_direction < 0f) || (_player.rb.velocity.x < 0f && _horizontal_direction > 0f);
        _can_jump |= Input.GetButtonDown("Jump"); 
    }

    private void FixedUpdate()
    {
        check_ground_collision();
        move_character();
        
        if (_on_ground)
        {
            apply_ground_linear_drag();
            crouch();
            _extra_jumps_count = _extra_jumps;
            _player.rb.gravityScale = 1f;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsAirborn", false);
        }
        else
        {
            apply_air_linear_drag();
            set_gravity();
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsAirborn", true);
        }

        if (_can_jump && _extra_jumps_count > 0) jump();
        _can_jump = false;
    }

///Return a Vector 2 that contains the horizontal input and place it on the X value and the horizontal input and place it on the Y value
    private Vector2 get_input()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    ///Move the character on the horizontal direction, if the character is faster than _max_move_speed we clamp it at max speed
    private void move_character()
    {
        //Check if the player doesn't move, the character is moving by a default speed
        if(_horizontal_direction == 0)
        {
            _player.rb.AddForce(new Vector2(1, 0f) * _default_speed);
        }
        else
        {
            _player.rb.AddForce(new Vector2(_horizontal_direction, 0f) * _movement_acceleration);
        }
        if(Mathf.Abs(_player.rb.velocity.x) > _max_move_speed)
        {
            _player.rb.velocity = new Vector2(Mathf.Sign(_player.rb.velocity.x) * _max_move_speed, _player.rb.velocity.y);
        }
    }

    ///Check is the player is crouching and change his collider size if he is
    private void crouch()
    {   
        if(_vertical_direction < 0)
        {
            GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
            animator.SetBool("IsCrouching", true);
            _is_crouching = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(1f, 0.97f);
            animator.SetBool("IsCrouching", false);
            _is_crouching = false;
        }
        
    }

    ///Apply the ground linear drag to the character
    private void apply_ground_linear_drag()
    {
        if(Mathf.Abs(_horizontal_direction) < 0.4f || _changing_direction)
        {
            _player.rb.drag = _ground_linear_drag;
        }
        else
        {
            _player.rb.drag = 0f;
        }
    }

    ///Apply the air linear drag to the character
     private void apply_air_linear_drag()
    {
        _player.rb.drag = _air_linear_drag;
    }


    ///It makes the character jump. If the character is not on ground, it retrieves him an extra jump
    /// then if the player is crouching, the player jump a little bit higher (25% as we reduce his size by 50% (25% top side, 25% bot side))

    private void jump()
    {
        if (!_on_ground)
        {
            _extra_jumps_count--;
            animator.SetBool("IsAirborn", true);
        }

        if (_is_crouching)
        {
            _player.rb.velocity = new Vector2(_player.rb.velocity.x, 0f);
            _player.rb.AddForce(Vector2.up * 1.25f * _jump_force, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsAirborn", true);
        }
        else
        {
            _player.rb.velocity = new Vector2(_player.rb.velocity.x, 0f);
            _player.rb.AddForce(Vector2.up * _jump_force, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsAirborn", true);
        }
        
    }

    ///This function set the value of the gravity.
    ///The value depends of the player movement (jump cut, fast fall, jump)
    private void set_gravity()
    {    
        //jump Cut
        if ( _player.rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _player.rb.gravityScale = _low_jump_fall_gravity;
        }
        //fast fall
        else if(_player.rb.velocity.y < 0f && _vertical_direction <0)
        {
             _player.rb.gravityScale = _fast_fall_gravity;
            if(Mathf.Abs(_player.rb.velocity.y) > _fastfall_max_speed)
            {
                _player.rb.velocity = new Vector2(_player.rb.velocity.x, Mathf.Sign(_player.rb.velocity.y) * _fastfall_max_speed);
                
            }
        } 
        //jump
        else if(_player.rb.velocity.y < 0f)
        {
            _player.rb.gravityScale = _fall_gravity;
        }   
        else
        {
            _player.rb.gravityScale = 1f;
        }

       

    }


    /// To check the ground collision, we cast rays from the player to the ground.
    /// One in front of the player and the other in his back to still detect the ground if the player is on the edge of the ground.
    private void check_ground_collision()
    {
        //if(_is_crouching)
        //{
        //    _on_ground = Physics2D.Raycast(transform.position + _ground_raycast_offset , Vector2.down, _ground_raycast_length * 0.5f, _ground_layer) ||
        //            Physics2D.Raycast(transform.position - _ground_raycast_offset , Vector2.down, _ground_raycast_length * 0.5f, _ground_layer);
        //    animator.SetBool("IsAirborn", false);
        //    animator.SetBool("IsJumping", false);
        //}
        //else
        {
            _on_ground = Physics2D.Raycast(transform.position + _ground_raycast_offset , Vector2.down, _ground_raycast_length, _ground_layer) ||
                    Physics2D.Raycast(transform.position - _ground_raycast_offset , Vector2.down, _ground_raycast_length, _ground_layer);
            animator.SetBool("IsAirborn", false);
            animator.SetBool("IsJumping", false);
        }
        
    }

    //These guizmos show the ray that is cast to check the ground collision
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        if(_is_crouching)
        {
            Gizmos.DrawLine(transform.position + _ground_raycast_offset*2, transform.position + _ground_raycast_offset + Vector3.down * _ground_raycast_length * 0.5f);
            Gizmos.DrawLine(transform.position - _ground_raycast_offset, transform.position - _ground_raycast_offset + Vector3.down * _ground_raycast_length * 0.5f);
            animator.SetBool("IsCrouching", true);

        }
        else
        {
            Gizmos.DrawLine(transform.position + _ground_raycast_offset*3, transform.position + _ground_raycast_offset*3 + Vector3.down * _ground_raycast_length);
            Gizmos.DrawLine(transform.position - _ground_raycast_offset, transform.position - _ground_raycast_offset + Vector3.down * _ground_raycast_length);
            animator.SetBool("IsCrouching", false);
        }
         

    }

    /*void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void FlipSword()
    {
        GameObject child = gameObject.transform.GetChild(0).gameObject;

        Vector3 currentScale = child.transform.localScale;
        currentScale.x *= -1;
        child.transform.localScale = currentScale;
    }*/
}
