using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStateMachine : MonoBehaviour
{

    public enum PlayerState
    {
        IDLE,WALK,RUN,JUMP,FALL,ATTACK,DASH
    }

    public PlayerState currentState;
    public Rigidbody2D m_rb2d;
    public Animator m_animator;
    [SerializeField] TrailRenderer m_trailRenderer;

    [Header( "Speeds" )]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 15f;
    public float fallGravityScale = 2.5f;
    public int maxJumps = 2;

    [Header( "Dash" )]
    public float dashDuration = 0.3f;
    public float dashSpeed = 15f;

    [Header( "Ground Detection" )]
    public LayerMask groundLayer;
    public Vector2 groundCheckerSize = Vector2.one;
    public Transform groundCheckerTransform;

    public Transform cameraAnchor;

    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _isAttacking = false;
    private bool _isDashing = false;
    private bool _canDash = false;
    private Vector2 _direction = Vector2.zero;
    private float _currentSpeed = 0f;
    private bool _jumpBuffer = false;
    private bool _isGrounded = false;
    public bool attackFinish = false;
    private int jumpQty = 0;
    private float dashChrono = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if( _direction.x != 0 )
        {
            transform.localScale = new Vector3( _direction.x, transform.localScale.y, transform.localScale.z );
        }

        Collider2D ground = Physics2D.OverlapBox( groundCheckerTransform.position, groundCheckerSize, 0f, groundLayer );

        Vector3 desiredPosition = transform.position + (Vector3) m_rb2d.velocity;

        Vector3 velocity = Vector3.zero;
        cameraAnchor.position = Vector3.SmoothDamp( cameraAnchor.position, desiredPosition, ref velocity, 0.1f );

        if( ground != null )
        {
            _isGrounded = true;
            _canDash = true;
        }else
        {
            _isGrounded = false;
        }

        OnStateUpdate();
    }

    private void FixedUpdate()
    {

        if ( _jumpBuffer )
        {
            m_rb2d.velocity = new Vector2( m_rb2d.velocity.x, jumpForce );
            _jumpBuffer = false;
            jumpQty++;
        }
    }


    private void OnDrawGizmos()
    {
        if( _isGrounded )
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube( groundCheckerTransform.position, groundCheckerSize );
    }

    private void OnStateEnter()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                m_animator.SetFloat( "velocityX", 0f );
                break;
            case PlayerState.WALK:
                _currentSpeed = walkSpeed;
                m_animator.SetFloat( "velocityX", 0.5f );
                break;
            case PlayerState.RUN:
                _currentSpeed = runSpeed;
                m_animator.SetFloat( "velocityX", 1f );
                break;
            case PlayerState.JUMP:
                // m_rb2d.velocity = new Vector2( m_rb2d.velocity.x, jumpForce );
                _jumpBuffer = true;
                m_animator.SetInteger( "velocityY", 1 );

                break;
            case PlayerState.FALL:
                m_animator.SetInteger( "velocityY", -1 );
                m_rb2d.gravityScale = fallGravityScale;
                break;
            case PlayerState.ATTACK:
                _currentSpeed = 0f;
                attackFinish = false;
                m_animator.SetBool( "Attacking", true );
                break;
            case PlayerState.DASH:
                dashChrono = 0f;
                m_rb2d.gravityScale = 0f;
                _canDash = false;
                _isDashing = false;
                m_trailRenderer.emitting = true;
                m_animator.SetBool( "Dashing", true );
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:

                if( !_isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.WALK );
                }
                else if ( _isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if( _isJumping && _isGrounded )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( m_rb2d.velocity.y < 0f && !_isGrounded )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if( _isAttacking && _isGrounded )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if( _isDashing && _canDash )
                {
                    TransitionToState( PlayerState.DASH );
                }

                break;
            case PlayerState.WALK:

                m_rb2d.velocity = new Vector2( _direction.x * _currentSpeed, m_rb2d.velocity.y );

                if ( _direction.magnitude == 0f )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                else if ( _isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if ( _isJumping && _isGrounded )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( _isAttacking && _isGrounded )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if ( m_rb2d.velocity.y < 0f && !_isGrounded )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if ( _isDashing && _canDash )
                {
                    TransitionToState( PlayerState.DASH );
                }

                break;
            case PlayerState.RUN:

                m_rb2d.velocity = new Vector2( _direction.x * _currentSpeed, m_rb2d.velocity.y );

                if ( _direction.magnitude == 0f )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                else if ( !_isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.WALK );
                }
                else if ( _isJumping && _isGrounded )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( _isAttacking && _isGrounded )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if ( m_rb2d.velocity.y < 0f && !_isGrounded )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if ( _isDashing && _canDash )
                {
                    TransitionToState( PlayerState.DASH );
                }

                break;
            case PlayerState.JUMP:

                m_rb2d.velocity = new Vector2( _direction.x * _currentSpeed, m_rb2d.velocity.y );

                if ( m_rb2d.velocity.y < 0f && !_isGrounded && !_jumpBuffer )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if ( _isDashing && _canDash )
                {
                    TransitionToState( PlayerState.DASH );
                }

                break;
            case PlayerState.FALL:

                m_rb2d.velocity = new Vector2( _direction.x * _currentSpeed, m_rb2d.velocity.y );

                if ( _isGrounded )
                {
                    jumpQty = 0;
                    if ( _direction.magnitude == 0f )
                    {
                        TransitionToState( PlayerState.IDLE );
                    }
                    else if ( !_isRunning && _direction.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.WALK );
                    }
                    else if ( _isRunning && _direction.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.RUN );
                    }
                }
                else if ( jumpQty < maxJumps && _isJumping )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( _isDashing && _canDash )
                {
                    TransitionToState( PlayerState.DASH );
                }

                break;
            case PlayerState.ATTACK:

                if( attackFinish )
                {
                    if ( _direction.magnitude == 0f )
                    {
                        TransitionToState( PlayerState.IDLE );
                    }
                    else if ( !_isRunning && _direction.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.WALK );
                    }
                    else if ( _isRunning && _direction.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.RUN );
                    }
                    else if ( _isDashing && _canDash )
                    {
                        TransitionToState( PlayerState.DASH );
                    }
                }

                break;
            case PlayerState.DASH:
                dashChrono += Time.deltaTime;

                m_rb2d.velocity = new Vector2( transform.localScale.x * dashSpeed, 0f );

                if( dashChrono >= dashDuration )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALK:
                break;
            case PlayerState.RUN:
                break;
            case PlayerState.JUMP:
                break;
            case PlayerState.FALL:
                m_animator.SetInteger( "velocityY", 0 );
                m_rb2d.gravityScale = 1f;
                break;
            case PlayerState.ATTACK:
                m_animator.SetBool( "Attacking", false );
                break;
            case PlayerState.DASH:
                m_rb2d.velocity = Vector2.zero;
                m_rb2d.gravityScale = 1f;
                m_trailRenderer.emitting = false;
                m_animator.SetBool( "Dashing", false );
                break;
            default:
                break;
        }
    }

    private void TransitionToState( PlayerState newState )
    {
        OnStateExit();
        currentState = newState;
        OnStateEnter();
    }

    public void RunButton( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Performed:
                _isRunning = true;
                break;
            case InputActionPhase.Canceled:
                _isRunning = false;
                break;
            default:
                break;
        }
    }

    public void JumpButton( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Performed:
                _isJumping = true;
                break;
            case InputActionPhase.Canceled:
                _isJumping = false;
                break;
            default:
                break;
        }
    }

    public void AttackButton( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Performed:
                _isAttacking = true;
                break;
            case InputActionPhase.Canceled:
                _isAttacking = false;
                break;
            default:
                break;
        }
    }

    public void MoveDirection( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Performed:
                _direction = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                _direction = Vector2.zero;
                break;
            default:
                break;
        }
    }

    public void Dash( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Performed:
                _isDashing = true;
                break;
            case InputActionPhase.Canceled:
                _isDashing = false;
                break;
        }
    }
}
