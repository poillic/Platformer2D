using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{

    public enum PlayerState
    {
        IDLE,WALK,RUN,JUMP,FALL,ATTACK
    }

    public PlayerState currentState;
    public Rigidbody2D m_rb2d;
    public Animator m_animator;

    [Header( "Speeds" )]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 15f;

    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _isAttacking = false;
    private Vector2 _direction = Vector2.zero;
    private float _currentSpeed = 0f;
    private bool _jumpBuffer = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }

    private void FixedUpdate()
    {

        if ( _jumpBuffer )
        {
            m_rb2d.velocity = new Vector2( m_rb2d.velocity.x, jumpForce );
            _jumpBuffer = false;
        }

        m_rb2d.velocity = new Vector2( _direction.x * _currentSpeed, m_rb2d.velocity.y );
    }

    private void OnStateEnter()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                _currentSpeed = 0f;
                break;
            case PlayerState.WALK:
                _currentSpeed = walkSpeed;
                break;
            case PlayerState.RUN:
                _currentSpeed = runSpeed;
                break;
            case PlayerState.JUMP:
                // m_rb2d.velocity = new Vector2( m_rb2d.velocity.x, jumpForce );
                _jumpBuffer = true;
                break;
            case PlayerState.FALL:
                break;
            case PlayerState.ATTACK:
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
                else if( _isJumping )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if( _isAttacking )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if ( m_rb2d.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }

                break;
            case PlayerState.WALK:

                if ( _direction.magnitude == 0f )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                else if ( _isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if ( _isJumping )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( _isAttacking )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if ( m_rb2d.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }

                break;
            case PlayerState.RUN:

                if ( _direction.magnitude == 0f )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                else if ( !_isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.WALK );
                }
                else if ( _isJumping )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if ( _isAttacking )
                {
                    TransitionToState( PlayerState.ATTACK );
                }
                else if ( m_rb2d.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }

                break;
            case PlayerState.JUMP:

                if ( m_rb2d.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }

                break;
            case PlayerState.FALL:

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

                break;
            case PlayerState.ATTACK:

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
                break;
            case PlayerState.ATTACK:
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

    private void OnDrawGizmos()
    {
    }
}
