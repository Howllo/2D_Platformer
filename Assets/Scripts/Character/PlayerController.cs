using System;
using System.Collections;
using MiscScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public enum ECharacterState
        {
            ECharNormal,
            ECharDashing,
            ECharJumping,
            ECharDoubleJump,
            ECharStunned,
            ECharDead
        }

        [Header("Set Variables")] 
        [SerializeField] private PlayerCharacter pc;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CircleCollider2D groundC2D;
        public Animator anim;
        [SerializeField] private SpriteRenderer sr;
        public ECharacterState state;
        private float pInputX, pInputY;
        private int RawX, RawY;
    

        // Start is called before the first frame update
        void Start()
        {
            state = ECharacterState.ECharNormal;
            isGrounded = true;
            Instantiate(dashEffect);
        }
    
        // Update is called once per frame
        void Update()
        {
            if (isGrounded)
                coyoteTimeCountDown = coyoteTime;
            else
                coyoteTimeCountDown -= Time.deltaTime;
            
            Jump();
            SetAxis();
            ChangeJumpToFallAnimation();
            GroundDetection();
            CharMovement();
            SetSpriteRenderX();
            Dashing();
            Crouch();
            Settings();
        }

        private void FixedUpdate()
        {
            SetMaxVelocity();
        }

        private void SetAxis()
        {
            RawX = (int)Input.GetAxisRaw("Horizontal");
            RawY = (int)Input.GetAxisRaw("Horizontal");
            pInputX = Input.GetAxis("Horizontal");
            pInputY = Input.GetAxis("Vertical");
        }

        private void SetMaxVelocity()
        {
            float speed = Vector3.Magnitude(rb.velocity);
            if (speed > jumpForce)
            {
                Vector2 normalized = rb.velocity.normalized;
                Vector2 newVelocity = normalized * jumpForce;
                rb.AddForce(newVelocity);
            }
        }
        
        private bool CheckForBadState()
        {
            switch (state)
            {
                case ECharacterState.ECharNormal:
                    return false;
                case ECharacterState.ECharDashing:
                    return true;
                case ECharacterState.ECharJumping:
                    return false;
                case ECharacterState.ECharDoubleJump:
                    return false;
                case ECharacterState.ECharDead:
                    return true;
                case ECharacterState.ECharStunned:
                    return true;
            }
            return false;
        }
        
        
        #region Grounding

        [Header("Ground")] 
        public bool isGrounded;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private BoxCollider2D detect;
        private static readonly int Falling = Animator.StringToHash("Falling");

        private void GroundDetection()
        {
            bool grounded = detect.IsTouchingLayers(groundMask);

            if (!isGrounded && grounded)
            {
                isGrounded = true;
                state = ECharacterState.ECharNormal;
                groundC2D.offset = new Vector2(-0.009565465f, -0.097f);
            } else if (isGrounded && !grounded)
            {
                isGrounded = false;
                groundC2D.offset = new Vector2(-0.009565465f, -0.05f);
            }
        }
    
        #endregion

        #region Movement

        [Header("Movement")] 
        [SerializeField] private float walkSpeed = 10;
        [SerializeField] private float accelerationStart = 2;
        [SerializeField] private float movementLerp = 100;
        [SerializeField] private float movementOffset = 2.6f;
        private static readonly int Walking = Animator.StringToHash("Walking");

        private void CharMovement()
        {
            SetSpriteRenderX();
            
            if( rb.velocity.x == 0)
                anim.SetBool(Walking, false);
            else if (rb.velocity.x < (movementOffset * -1) && isGrounded || rb.velocity.x > movementOffset && isGrounded)
                anim.SetBool(Walking, true);

            if (CheckForBadState()) return;

            var acceleration = isGrounded ? accelerationStart : accelerationStart * 0.5f;
            
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (rb.velocity.x > 0) pInputX = 0;
                pInputX = Mathf.MoveTowards(pInputX, -1, acceleration * Time.deltaTime);
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (rb.velocity.x < 0) pInputX = 0;
                pInputX = Mathf.MoveTowards(pInputX, 1, acceleration * Time.deltaTime);
            }
            else
                pInputX = Mathf.MoveTowards(pInputX, 0, acceleration * 2 * Time.deltaTime);
        
            var newVelocity = new Vector3(pInputX * walkSpeed, rb.velocity.y);
            rb.velocity = Vector3.MoveTowards(rb.velocity, newVelocity, movementLerp * Time.deltaTime);
        }

        private void SetSpriteRenderX()
        {
            if (rb.velocity.x < -0.001f)
                sr.flipX = true;
            else if (rb.velocity.x > 0.001f)
                sr.flipX = false;
        }
        
        #endregion

        #region Jump

        [Header("Jumping")] 
        [SerializeField] private float jumpForce = 40;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float coyoteTimeCountDown;
        [SerializeField] private bool doubleJump = true;
        [SerializeField] private AudioClip doubleJumpClip;
        [SerializeField] private ParticleSystem doubleJumpSmoke;
        private static readonly int Jumping = Animator.StringToHash("Jumping");

        private void Jump()
        {
            if (state == ECharacterState.ECharStunned || state == ECharacterState.ECharDoubleJump) return;

            if (Input.GetButtonDown("Jump"))
            {
                if (coyoteTimeCountDown > 0f || doubleJump && state == ECharacterState.ECharJumping)
                {
                    state = state == ECharacterState.ECharJumping ? ECharacterState.ECharDoubleJump : ECharacterState.ECharJumping;
                    if (state == ECharacterState.ECharDoubleJump)
                    {
                        AudioManager.Instance.PlaySoundEffect(doubleJumpClip);
                        doubleJumpSmoke.transform.position = transform.position;
                        Instantiate(doubleJumpSmoke);
                    }
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }

        private void ChangeJumpToFallAnimation()
        {
            if (rb.velocity.y < 0 && !isGrounded)
            {
                anim.SetBool(Jumping, false);
                anim.SetBool(Falling, true);
            }
            else if (rb.velocity.y > 0 && !isGrounded)
            {
                coyoteTimeCountDown = 0f;
                anim.SetBool(Jumping, true);
                anim.SetBool(Falling, false);
            }
            else
            {
                anim.SetBool(Jumping, false);
                anim.SetBool(Falling, false);
            }
        }
    
        #endregion

        #region Dashing

        [Header("Dash")]
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashLength;
        [SerializeField] private float dashingPower;
        [SerializeField] private bool canDash = true;
        [SerializeField] private AudioClip dashClip;
        [SerializeField] private ParticleSystem dashEffect;
        private float timeStartDashing;
        private float originalGravity;
        
        private void Dashing()
        {
            if (CheckForBadState()) return;
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash || Input.GetKeyDown(KeyCode.RightControl) && canDash)
            {
                
                originalGravity = rb.gravityScale;
                state = ECharacterState.ECharDashing;
                anim.SetBool(Crouching, true);
                canDash = false;
                rb.gravityScale = 0f;
                dashEffect.Play();
                AudioManager.Instance.PlaySoundEffect(dashClip);
                timeStartDashing = Time.time;
            }

            if (!canDash)
            {
                if (Time.time >= timeStartDashing + dashLength)
                {
                    anim.SetBool(Crouching, false);
                    rb.gravityScale = originalGravity;
                    state = ECharacterState.ECharNormal;
                    canDash = true;
                    dashEffect.Stop();
                }
            }
        }

        #endregion

        #region Crouch
    
        private static readonly int Crouching = Animator.StringToHash("Crouching");
    
        private void Crouch()
        {
            if(CheckForBadState()) return;
            
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                anim.SetBool(Crouching, true);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool(Crouching, false);
            }
        }

        #endregion

        #region Settings

        [Header("Setting")]
        [SerializeField] private bool isSettingActive;
        
        private void Settings()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isSettingActive)
                {
                    pc.GetPlayerUserInterface().settingsUI.SetActive(true);
                    state = ECharacterState.ECharStunned;
                    Time.timeScale = 0;
                    isSettingActive = true;
                }
                else if (isSettingActive)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        pc.GetPlayerUserInterface().settingsUI.SetActive(false);
                        state = ECharacterState.ECharNormal;
                        Time.timeScale = 1;
                        isSettingActive = false;
                    }
                }
            }
        }

        #endregion
        
        public Rigidbody2D GetRb()
        {
            return rb;
        }
    }
}