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

    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _isAttacking = false;
    private Vector2 _direction = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnStateEnter()
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

    private void OnStateUpdate()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:

                if( _isRunning && _direction.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
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
}
