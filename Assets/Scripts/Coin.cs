using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent OnPickUp;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if( collision.CompareTag( "Player" ) )
        {
            OnPickUp.Invoke();
            Destroy( gameObject, 1f );
        }
    }
}
