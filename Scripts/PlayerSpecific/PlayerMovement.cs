using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float dashSpeed = 10f;
    [SerializeField] public float dashTime = 0.2f;
    [SerializeField] public float dashCooldown = 1f;
    private float startingMoveSpeed = 0f;

    Vector2 rawInput;
    Vector2 delta = new Vector2(0,0);
    [SerializeField] Rigidbody2D myRigidbody;

    [SerializeField] float paddingLeft = 0.5f;
    [SerializeField] float paddingRight = 0.5f;
    [SerializeField] float paddingTop = 5f;
    [SerializeField] float paddingBottom = 2f;
    
    bool isDashing = false;
    private bool canDash = false;

    public UnityEvent pauseGame;

    //Shooter shooter;
    WeaponsManagement mWeaponsManagement;
    Vector2 minBounds;
    Vector2 maxBounds;

    //Temporary Player State
    public bool isStunned = false;

    private void Awake()
    {
        mWeaponsManagement = GetComponent<WeaponsManagement>();
        startingMoveSpeed = moveSpeed;
    }

    private void Start()
    {
        InitBounds();
    }

    // Update is called once per frame
    void Update()
    {
       Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        //Viewport is normalized
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    private void Move()
    {
        delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        //Clamping and preventing player from being able to move out of bounds
        newPos.x = Mathf.Clamp(transform.position.x + delta.x,minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (isStunned) return;
        Debug.Log("Firing");
        if(mWeaponsManagement != null)
        {
            mWeaponsManagement.isFiring = value.isPressed;
        }
    }

    void OnWeapon1(InputValue value)
    {
        //WeaponsManagement switch selected weapon to primary
        mWeaponsManagement.SetEquipped(1);
        Debug.Log("One pressed");
    }

    void OnWeapon2(InputValue value)
    {
        mWeaponsManagement.SetEquipped(2);
        Debug.Log("Two pressed");
    }

    void OnWeapon3(InputValue value)
    {
        mWeaponsManagement.SetEquipped(3);
        Debug.Log("Three pressed");
    }

    void OnPause(InputValue value)
    {
        Debug.Log("Pause Pressed");
        pauseGame.Invoke();
    }

    public void OnDash(InputValue value)
    {
        Debug.Log("Dashing");
        //if (!isAlive || CheckGround()) { return; }
        Dash();

    }

    IEnumerator PlayerStunned(float stunnedTime)
    {
        isStunned = true;
        Debug.Log("Player is stunned");
        yield return new WaitForSeconds(stunnedTime);
        isStunned = false;
    }

    IEnumerator PlayerRapidFire(float rapidTime)
    {
        float oldFiringRate = mWeaponsManagement.GetFiringRate();
        isStunned = true;
        Debug.Log("Player is in rapid fire mode");
        yield return new WaitForSeconds(rapidTime);
        isStunned = false;
    }

    /*
    IEnumerator Dash()
    {
        /*This is for celeset like dashing, need to determine your vertical direction, likely from move.y for vertical
            //float horz = Input.GetAxisRaw("Horizontal");
            //float vert = Input.GetAxisRaw("Vertical");
            //Vector2 dir = new Vector2(horz, vert).normalized;
            myRigidbody.velocity = new Vector2(dir.x * dashSpeed, dir.y * dashSpeed);
       
        Vector2 dashDirection = new Vector2(rawInput.x, rawInput.y);
        //isDashing = false;
        float gravity = myRigidbody.gravityScale;
        Debug.Log("Starting couroutine");
        //Wall Dash?
        //Shoot out a ray when dashing, it hits a wall and the player presses the jump button in time
        if (canDash)
        {
            Debug.Log("Move input while dashing" + rawInput.x);
            //myRigidbody.velocity = new Vector2(7.0f, 0f);
            //myRigidbody.AddForce(new Vector2(dashDistance * dSirection * -7, 30), ForceMode2D.Impulse);

            Debug.Log(myRigidbody.velocity);
            myRigidbody.gravityScale = 0;

            myRigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        }
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        myRigidbody.gravityScale = gravity;
        isDashing = false;
        Debug.Log("Done dashing");
    } */

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            //myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        Debug.Log("Dashing");
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        //myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Electric")
        {
            //Eventually should retrieve value from stunned item
            StartCoroutine(PlayerStunned(2f));
        }
        if(other.tag == "RapidFire")
        {
            StartCoroutine(PlayerRapidFire(5f));
        }
    }

}
